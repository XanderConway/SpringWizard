using System;
using UnityEngine;
using UnityEngine.InputSystem;

/*
 * POGO STICK CONTROLS
 * WS -> Rotate forward and backwards
 * AD -> Rotate side to side
 * SPACE -> Hold space when on the ground to compress the spring longer and jump higher
 * MOUSE -> Pan to change the look direction
 * Q -> Perform trick 1 (Needs to be performed with a flip for effect)
 * E -> Perform trick 2 (Needs to be performed with a flip for effect)
 */

public class PogoControls : PlayerSubject, TimerObserver
{
    // Rotation parameters
    public float rotationSpeed = 360f;
    public float leanSpeed = 360f;
    public float maxLeanAngle = 15;
    public float maxLeanForwardAngle = 60;
    public float maxLeanBackwardAngle = 40;

    private float currentLeanAngle = 0;
    private float currentFlipAngle = 0;
    private float fireJumpBoost = 100;
    public AnimationCurve flipSpeedMultiplier;

    // Trick detection parameters
    private int currTrick = 0;
    private int flipType = 0;
    private Vector3 prevPogoUp = Vector3.up;

    // Jump detection parameters
    public float pogoRayCastLength = 10f;
    public Vector3 pogoRayCastOffset = Vector3.zero;

    // The axis to rotate the player around when doing flips in the air
    public Vector3 flipAxisOffset = Vector3.zero;

    // Parameters for jump forces
    public float baseJumpForce = 1.0f;
    public float maxChargedJumpForce = 1.0f;
    public float compressTime = 0.4f;

    // Used for spring compression animation (Purely Aesthetic)
    public GameObject mainPogoBody;
    public float springLength = 1.0f;
    public float springMaxCompression = 0.003f;
    private Vector3 pogoBodyHeightOffGround;

    // To handle player Death
    public float lethalImpactThreshold = 0;
    public GameObject ragdollBody;
    public GameObject[] pogoStickComponents;
    public Camera cam; //Hacky fix for camera issues
    public GameObject deathPogoStick; // Disable the acutal pogostick and replace it with a dummy on death
    private bool dead = false;
    private Vector3 respawnPoint;
    private Rigidbody[] ragdollBones;

    // These won't be needed once we have an animator
    private Quaternion[] startBoneRotations;
    private Vector3[] startBonePositions;
    private Vector3 startCameraPosition;

    public AnimationCurve bounceScale;
    private float groundedTimer = 0;
    private bool grounded = false;

    // Transforms used for rotations
    public Transform pogoStick; // Will flip around it's side axis
    public Transform leanChild; // Will rotate around forward axis, should be the child of pogostick
    private Rigidbody rb;
    private PlayerInputActions playerInputActions;
    private Vector2 leanInputVector;
    private float maxChargeTime = 2.0f;
    private float chargeTime = 0.0f;
    private bool isChargingJump = false;

    public AudioClip[] jumpFxs;
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pogoBodyHeightOffGround = mainPogoBody.transform.localPosition;
        ragdollBones = ragdollBody.GetComponentsInChildren<Rigidbody>();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Lean.Enable();
        playerInputActions.Player.ChargeJump.Enable();
        playerInputActions.Player.ChargeJump.performed += OnChargeJumpStarted;
        playerInputActions.Player.ChargeJump.canceled += OnChargeJumpReleased;
        playerInputActions.Player.Restart.Enable();
        playerInputActions.Player.Restart.performed += OnRestart;

        startCameraPosition = cam.transform.localPosition;
        startBoneRotations = new Quaternion[ragdollBones.Length];
        startBonePositions = new Vector3[ragdollBones.Length];
        for (int i = 0; i < ragdollBones.Length; i++)
        {
            {
                startBoneRotations[i] = ragdollBones[i].transform.localRotation;
                startBonePositions[i] = ragdollBones[i].transform.localPosition;
            }
        }

        ToggleRagdoll(false);

        NotifyTrickObservers(PlayerTricks.None);
    }

    void OnRestart(InputAction.CallbackContext context)
    {
        transform.position = new Vector3(-1625, 900, -456);
    }

    void OnChargeJumpStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Jump held");
        isChargingJump = true;
        chargeTime = 0.0f;
    }

    private void OnChargeJumpReleased(InputAction.CallbackContext context)
    {
        //float jumpForce = Mathf.Lerp(baseJumpForce, maxChargedJumpForce, chargeTime / maxChargeTime);
        //Jump(jumpForce);
        Debug.Log("Jump released");
        isChargingJump = false;
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            detectJumping();
            countFlips();
        }
        else if (isChargingJump)
        {
            dead = false;
            setDead(false);
            transform.position = respawnPoint + Vector3.up * 10;
            transform.rotation = Quaternion.identity;

            pogoStick.transform.localPosition = Vector3.zero;
            pogoStick.rotation = Quaternion.identity;

            leanChild.transform.localPosition = Vector3.zero;
            leanChild.rotation = Quaternion.identity;
        }
    }

    private void Update()
    {
        handleControls();
        if (!dead)
        {
            rotatePlayer();
        }
    }

    // Called when the pogo stick hits the ground
    void groundedEvent()
    {
        GameObject effect = null;
        
        if (flipType > 0)
        {
            if (currTrick > 0)
            {
                // Debug.Log("ICE");
                NotifyTrickObservers(PlayerTricks.NoHandsFrontFlip);

            }
            else if (currTrick < 0)
            {
                NotifyTrickObservers(PlayerTricks.NoFeetFrontFlip);
                // Debug.Log("EARTH");
            }
            else
            {
                NotifyTrickObservers(PlayerTricks.FrontFlip);
            }
        }
        else if (flipType < 0)
        {
            {
                if (currTrick > 0)
                {
                    // Debug.Log("FIRE");
                    NotifyTrickObservers(PlayerTricks.NoHandsBackFlip);
                }
                else if (currTrick < 0)
                {
                    // Debug.Log("AIR");
                    NotifyTrickObservers(PlayerTricks.NoFeetBackFlip);
                }
                else
                {
                    NotifyTrickObservers(PlayerTricks.BackFlip);
                }
            }
        }
        
        

        if(effect)
        {
            Destroy(effect, 1.5f);
        }

        currTrick = 0;
        flipType = 0;
    }

    private void Jump(float force)
    {
        rb.AddForce(leanChild.transform.up * force, ForceMode.Impulse);
        pogoStick.transform.localScale = Vector3.one;
        Physics.IgnoreLayerCollision(3, 4, true);


        if (jumpFxs.Length > 0 && audioSource)
        {
            int choice = UnityEngine.Random.Range(0, jumpFxs.Length);
            audioSource.PlayOneShot(jumpFxs[choice]);
        }
    }

    float jumpForce = 0;
    public float velocitySpringBonus = 10f;
    void detectJumping()
    {
        Vector3 pogoCastStart = leanChild.transform.position + leanChild.transform.rotation * pogoRayCastOffset;
        Vector3 pogoCastEnd = pogoCastStart + leanChild.transform.rotation * leanChild.transform.up * (-pogoRayCastLength);
        RaycastHit hit;
        LayerMask layerMask = ~0; // Collide with every layer
        
        layerMask = ~LayerMask.GetMask("Water");
        
         if (flipType > 0)
        {
            if (currTrick > 0)
            {
                layerMask = ~0;
                Physics.IgnoreLayerCollision(3, 4, false);
            }
        }

        if (Physics.Raycast(pogoCastStart, -1 * leanChild.transform.up, out hit, pogoRayCastLength, layerMask))
        {
            // Compress the spring if there is ground below us and we are moving downwards
            if (!grounded && rb.velocity.y <= 0)
            {
                grounded = true;
                jumpForce = baseJumpForce;
                if (isChargingJump)
                {
                    jumpForce += maxChargedJumpForce;
                }
                groundedEvent();
            }
        }
        //else if ice trick is triggered
        

        if (grounded)
        {
            respawnPoint = transform.position;
            groundedTimer += Time.deltaTime;

            // Current compression is inital_velocity * cos(time)
            float maxCompression = jumpForce * springMaxCompression;
            float bounceAmount = maxCompression * (-0.5f * Mathf.Cos(groundedTimer / compressTime * 2 * Mathf.PI) + 0.5f);
            
            // The spring is fully compressed, begin decompressing
            if (bounceAmount > 1)
            {
                bounceAmount = 1;
                //groundedTimer = compressTime - groundedTimer;
            }

            mainPogoBody.transform.localPosition = pogoBodyHeightOffGround + springLength * Vector3.down * bounceAmount;

            //float squashFactor = bounceScale.Evaluate(groundedTimer / compressTime);
            //pogoStick.transform.localScale = new Vector3(1, squashFactor, 1);
        }

        if (groundedTimer > compressTime)
        {
            {
                Jump(jumpForce);

                groundedTimer = 0;
                fireJumpBoost = 0;
                grounded = false;
            }
        }
    }

    void countFlips()
    {
        if (!grounded)
        {
            float angleDiff = Vector3.SignedAngle(prevPogoUp, pogoStick.up, transform.right);
            currentFlipAngle += angleDiff;
        } else
        {
            currentFlipAngle = 0;
        }

        prevPogoUp = pogoStick.up;

        if (currentFlipAngle > 270)
        {
            Debug.Log("Front Flip!");
            flipType = 1;
            currentFlipAngle = 0;
        } 

        if (currentFlipAngle < -270)
        {
            Debug.Log("Back Flip!");
            flipType = -1;
            currentFlipAngle = 0;
        }
    }

    void rotatePlayer()
    {
        leanInputVector = playerInputActions.Player.Lean.ReadValue<Vector2>();
        float forwardInput = leanInputVector.y;
        float sideInput = leanInputVector.x;

        float flipAngle = forwardInput * rotationSpeed * Time.deltaTime;

        // Change flip speed based on the angle, we want to move faster when upside down
        float dotProduct = Vector3.Dot(pogoStick.up.normalized, transform.up.normalized);
        flipAngle *= flipSpeedMultiplier.Evaluate(dotProduct);

        // Rotate around the foot of the pogo stick when grounded, and the center when in the air
        if (grounded)
        {
            float angleDiff = Vector3.SignedAngle(Vector3.up, pogoStick.up, transform.right);
            if (angleDiff > maxLeanForwardAngle || angleDiff < maxLeanBackwardAngle)
            {
                flipAngle = 0;
            }
            pogoStick.Rotate(transform.right, flipAngle, Space.World);
        }
        else
        {

            // TODO: Swap the leanChild and pogoStick in the hierarchy, use the pogostick rotation to fix camera issues
            // Use a raycast to prevent the pogostick from going into the ground
            Vector3 rotationCenter = pogoStick.transform.position + leanChild.transform.rotation * flipAxisOffset;
            pogoStick.RotateAround(rotationCenter, transform.right, flipAngle);
        }

        currentLeanAngle -= sideInput * leanSpeed * Time.deltaTime;

        // If there is no input, move the lean rotation back to 0
        if (Mathf.Abs(sideInput) < 0.1)
        {
            float leanAdjustment = leanSpeed * Time.deltaTime * 0.8f;
            currentLeanAngle -= Mathf.Sign(currentLeanAngle) * Mathf.Min(leanAdjustment, Mathf.Abs(currentLeanAngle));
        }

        currentLeanAngle = Mathf.Clamp(currentLeanAngle, -maxLeanAngle, maxLeanAngle);
        leanChild.localRotation = Quaternion.AngleAxis(currentLeanAngle, Vector3.forward);
    }

    void handleControls()
    {

        // Currently just changing colour as a place holder for animations
        if(Input.GetKey(KeyCode.Q))
        {
            currTrick = 1;
        } else if(Input.GetKey(KeyCode.E))
        {
            currTrick = -1;
        }
    }

    public void ToggleRagdoll(bool useRagdoll)
    {

        foreach (Rigidbody ragdollBone in ragdollBones)
        {

            ragdollBone.isKinematic = !useRagdoll;
            ragdollBone.gameObject.GetComponent<Collider>().enabled = useRagdoll;
        }


        for (int i = 0; i < pogoStickComponents.Length; i++)
        {
            pogoStickComponents[i].SetActive(!useRagdoll);
        }

        // This can be buggy and won't be needed once we have animations
        if (!useRagdoll)
        {
            for (int i = 0; i < ragdollBones.Length; i++)
            {
                ragdollBones[i].gameObject.transform.localRotation = startBoneRotations[i];
                ragdollBones[i].gameObject.transform.localPosition = startBonePositions[i];
                cam.transform.localPosition = startCameraPosition;
            }
        } else
        {
            //GameObject tempPogoStick = Instantiate(deathPogoStick, pogoStickComponents[1].transform);
            //Destroy(tempPogoStick, 1.0f);

        }
    }

    // Disable player model, and spawn rag doll.
    public void setDead(bool isDead)
    {
        dead = isDead;
        ToggleRagdoll(isDead);

    }

    public bool IsDead()
    {
        return dead;
    }


    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody attached to this object.");
            return;
        }

        ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
        collision.GetContacts(contactPoints);

        for (int i = 0; i < contactPoints.Length; i++)
        {   
            // Extremely hacky solution for now
            if (contactPoints[i].thisCollider.gameObject.name == "wizard_pose_v001")
            {
                float impactForce = collision.relativeVelocity.magnitude;

                if(impactForce > lethalImpactThreshold)
                {
                    Debug.Log("DEATH " + impactForce);
                    setDead(true);
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Draw the raycast line
        Vector3 pogoCastStart = leanChild.transform.position + leanChild.transform.rotation * pogoRayCastOffset;
        Vector3 pogoCastEnd = pogoCastStart + leanChild.transform.up * (-pogoRayCastLength);
        Gizmos.DrawLine(pogoCastStart, pogoCastEnd);

        // Draw the pogo stick center
        Gizmos.DrawSphere(pogoStick.transform.position + leanChild.transform.rotation * flipAxisOffset, 4);
    }

    public void ApplySpringboardForce(float force)
    {
        if (grounded)
        {
            jumpForce += force;
        }
        else
        {
            rb.AddForce(leanChild.transform.up * force, ForceMode.Impulse);
        }
    }

    //TODO Function dealing with timer
    public void UpdateTimeObserver(float time)
    {
        throw new NotImplementedException();
    }

    public void UpdateTimeRunning(bool isRunning)
    {
        throw new NotImplementedException();
    }

    public void TimesUP(bool timesUp)
    {   
        //TODO Implement action after Times up
        throw new NotImplementedException();
    }
}
