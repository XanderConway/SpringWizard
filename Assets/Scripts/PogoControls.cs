using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Splines;
using Unity.Mathematics;


public class DeathData
{
    public Vector3 lastGroundedPosition;
    public Vector3 deathPosition;
    public DeathData(Vector3 lastGroundedPosition, Vector3 deathPosition)
    {
        this.lastGroundedPosition = lastGroundedPosition;
        this.deathPosition = deathPosition;
    }
}

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
    public float pogoCastRadius = 40f;

    // The axis to rotate the player around when doing flips in the air
    public Vector3 flipAxisOffset = Vector3.zero;

    // Parameters for jump forces
    public float baseJumpForce = 1.0f;
    public float maxChargedJumpForce = 1.0f;
    public float maxCompressTime = 1.0f;
    public float compressTime = 0.4f;

    // Used for spring compression animation (Aesthetic)
    public GameObject mainPogoBody;
    public float velocitySpringMultiplier = 0.2f;
    public float springLength = 1.0f;
    public float springMaxCompression = 0.003f;
    private Vector3 pogoBodyHeightOffGround;
    public GameObject jumpParticle;

    // Parameters to handle Ragdoll and player death
    public float lethalImpactThreshold = 0;
    public GameObject ragdollBody; // The body of the character that will ragdoll on death
    public List<Collider> playerCollders; // Colliders that will be disabled when the player dies
    public GameObject[] pogoStickComponents;
    public GameObject deathCollider;
    public GameObject deathParticleEffect;
    private bool dead { get; set; }
    private UnityEvent<DeathData> deathEvent = new UnityEvent<DeathData>();
    private Rigidbody[] ragdollBones;
    private Quaternion[] startBoneRotations;
    private Vector3[] startBonePositions;

    private List<Collider> ragDollColliders;


    public Camera cam; //Hacky fix for camera issues
    private Vector3 camStartPosition;
    private Quaternion camStartRotation;

    private Vector3 lastGroundedPosition;

    public AnimationCurve bounceScale;
    private float groundedTimer = 0;
    private bool grounded = false;

    // Transforms used for rotations
    public Transform pogoStick; // Will flip around it's side axis
    private Vector3 pogoStickStartPosition; // Used to keep the pogostick a constant distance from it's parent 
    public Transform leanChild; // Will rotate around forward axis, should be the child of pogostick

    private Rigidbody rb;

    // Input parameters
    private PlayerInputActions playerInputActions;
    private Vector2 leanInputVector;
    private bool isChargingJump = false;

    public AudioClip[] jumpFxs;
    public AudioClip[] hurtFxs;
    public AudioClip[] flipFxs;

    // Rail grinding variables
    private bool isGrinding = false;
    private RailScript currentRailScript;
    private float grindElapsedTime;
    private float timeForFullSpline;
    private bool normalDir;
    public float grindSpeed = 15f; // 200 
    public float heightOffset = 1f; // 10
    public float lerpSpeed = 15f; // 200
    private bool reEnableCollidersPending = false;
    private float colliderReEnableDelay = 0.1f; 
    private float colliderReEnableTimer = 0f;
    
    public AudioSource pogoAudioSource;
    public AudioSource voiceAudioSource;


    public UnityEvent<DeathData> getDeathEvent()
    {
        return deathEvent;
    }

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


        startBoneRotations = new Quaternion[ragdollBones.Length];
        startBonePositions = new Vector3[ragdollBones.Length];
        for (int i = 0; i < ragdollBones.Length; i++)
        {
            {
                startBoneRotations[i] = ragdollBones[i].transform.localRotation;
                startBonePositions[i] = ragdollBones[i].transform.localPosition;
            }
        }

        camStartPosition = cam.transform.localPosition;
        camStartRotation = cam.transform.localRotation;

        pogoStickStartPosition = pogoStick.transform.localPosition;

        ToggleRagdoll(false);
        NotifyTrickObservers(PlayerTricks.None);
    }

    void OnRestart(InputAction.CallbackContext context)
    {

    }

    void OnChargeJumpStarted(InputAction.CallbackContext context)
    {
        isChargingJump = true;
    }

    private void OnChargeJumpReleased(InputAction.CallbackContext context)
    {
        if (isGrinding)
        {
        EndGrinding();
        // Not sure if needed, experiment with this
        rb.AddForce(Vector3.up * baseJumpForce, ForceMode.VelocityChange);
        }

        //float jumpForce = Mathf.Lerp(baseJumpForce, maxChargedJumpForce, chargeTime / maxChargeTime);
        //Jump(jumpForce);
        Debug.Log("Jump released");
        isChargingJump = false;
    }


    void FixedUpdate()
    {
        if (isGrinding)
        {
            MoveAlongRail();
        }
        else if (!dead)
        {
            detectJumping();
            countFlips();
           
        }
    }

    private void Update()
    {
    handleControls();

    // Very jank timer for a smoother jump at the end of a rail
    if (reEnableCollidersPending)
    {
        colliderReEnableTimer += Time.deltaTime;
        if (colliderReEnableTimer >= colliderReEnableDelay)
        {
            SetPlayerCollidersTrigger(false);
            Physics.IgnoreLayerCollision(3, 4, false);
            reEnableCollidersPending = false; 
        }
    }

    if (!dead)
    {
        if (!isGrinding)
        {
            rotatePlayer();
        }
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
                NotifyTrickObservers(PlayerTricks.NoHandsFrontFlip);

            }
            else if (currTrick < 0)
            {
                NotifyTrickObservers(PlayerTricks.NoFeetFrontFlip);
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
                    NotifyTrickObservers(PlayerTricks.NoHandsBackFlip);
                }
                else if (currTrick < 0)
                {
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


        if (jumpFxs.Length > 0 && pogoAudioSource)
        {
            int choice = UnityEngine.Random.Range(0, jumpFxs.Length);
            pogoAudioSource.PlayOneShot(jumpFxs[choice]);
        }
    }

    // TODO Clean up global variables
    float jumpForce = 0;
    private float chargedCompressTime = 0;

    float compressHalfTime;
    float decompressHalfTime;
    RaycastHit groundHit;
    void detectJumping()
    {
        Vector3 pogoCastStart = leanChild.transform.position + leanChild.transform.rotation * pogoRayCastOffset;
        Vector3 pogoCastEnd = pogoCastStart + leanChild.transform.rotation * leanChild.transform.up * (-pogoRayCastLength);
        RaycastHit hit;
        LayerMask layerMask = ~0; // Collide with every layer
        
        layerMask = ~LayerMask.GetMask("Player");

        if (Physics.SphereCast(pogoCastStart, pogoCastRadius, -1 * leanChild.transform.up, out hit, pogoRayCastLength, layerMask))
        {
            // Compress the spring if there is ground below us and we are moving downwards
            if (!grounded && rb.velocity.y <= 0)
            {
                grounded = true;
                jumpForce = baseJumpForce;

                jumpForce += Math.Min(Math.Abs(rb.velocity.y) * velocitySpringMultiplier, 100);

                chargedCompressTime = compressTime;

                compressHalfTime = compressTime / 2;
                decompressHalfTime = compressTime / 2;

                lastGroundedPosition = transform.position;

                groundHit = hit;
                groundedEvent();
            }
        }
        //else if ice trick is triggered
        

        if (grounded)
        {
            groundedTimer += Time.deltaTime;

            // For animations, we only want to slow down during compression, not while the spring is moving back up
            if (isChargingJump && groundedTimer < compressHalfTime && compressHalfTime < maxCompressTime)
            {
                // Compress the spring longer, and decompress faster, keeping the ratio the same
                compressHalfTime += Time.deltaTime;
                decompressHalfTime = compressTime / (compressHalfTime / compressTime);

                jumpForce += (Time.deltaTime / maxCompressTime) * maxChargedJumpForce;
            }

            // Current compression is inital_velocity * cos(time)

            float maxCompression = jumpForce * springMaxCompression;
            float bounceAmount;
            float squashFactor;
            if (groundedTimer < compressHalfTime)
            {
                bounceAmount = maxCompression * (-0.5f * Mathf.Cos(groundedTimer / compressHalfTime * Mathf.PI) + 0.5f);
                squashFactor = bounceScale.Evaluate(groundedTimer / (2 * compressHalfTime));
            } else
            {
                bounceAmount = maxCompression * (-0.5f * Mathf.Cos((groundedTimer - compressHalfTime) / decompressHalfTime * Mathf.PI + Mathf.PI) + 0.5f);
                squashFactor = bounceScale.Evaluate((groundedTimer - compressHalfTime) / (2 * decompressHalfTime) + 0.5f);
            }

            // The spring is fully compressed, begin decompressing
            bounceAmount = Mathf.Clamp(bounceAmount, 0, 1);

            mainPogoBody.transform.localPosition = pogoBodyHeightOffGround + springLength * Vector3.down * bounceAmount;

            pogoStick.transform.localScale = new Vector3(1, squashFactor, 1);
        }

        if (groundedTimer > compressHalfTime + decompressHalfTime)
        {
            {
                Jump(jumpForce);

                GameObject jumpEffect = Instantiate(jumpParticle, pogoStick.transform.position, Quaternion.FromToRotation(Vector3.up, groundHit.normal));
                Destroy(jumpEffect, 1.0f);

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
            flipType = 1;
            currentFlipAngle = 0;

            if(flipFxs.Length > 0)
            {
                pogoAudioSource.PlayOneShot(flipFxs[0]);
            }
        } 

        if (currentFlipAngle < -270)
        {
            flipType = -1;
            currentFlipAngle = 0;

            pogoAudioSource.PlayOneShot(flipFxs[1]);
        }
    }


    void rotatePlayer()
    {
        if (isGrinding)
        {
            return;
        }
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

    private void setCollidersActive(List<Collider> components, bool active)
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].enabled = active;
        }
    }

    public void ToggleRagdoll(bool useRagdoll)
    {

        foreach (Rigidbody ragdollBone in ragdollBones)
        {

            ragdollBone.isKinematic = !useRagdoll;
            ragdollBone.gameObject.GetComponent<Collider>().enabled = useRagdoll;
            ragdollBone.velocity = rb.velocity;
        }


        for (int i = 0; i < pogoStickComponents.Length; i++)
        {
            pogoStickComponents[i].SetActive(!useRagdoll);
        }

        // Setting bone positions manually can be buggy, and won't be needed once we have animation
        if (!useRagdoll)
        {
            for (int i = 0; i < ragdollBones.Length; i++)
            {
                ragdollBones[i].gameObject.transform.localRotation = startBoneRotations[i];
                ragdollBones[i].gameObject.transform.localPosition = startBonePositions[i];
                cam.transform.localPosition = camStartPosition;
                cam.transform.localRotation = camStartRotation;
                pogoStick.localPosition = pogoStickStartPosition;
            }

            setCollidersActive(playerCollders, true);
            rb.isKinematic = false;
        } else
        {
            setCollidersActive(playerCollders, false);
            rb.isKinematic = true;
        }
    }

    // Disable player model, and spawn rag doll.
    public void setDead(bool isDead)
    {
        if(isDead)
        {
            DeathData data = new DeathData(lastGroundedPosition, transform.position);
            deathEvent.Invoke(data);
            NotifyTrickObservers(PlayerTricks.Death);
        }
        dead = isDead;
        ToggleRagdoll(isDead);

    }

    public bool IsDead()
    {
        return dead;
    }

    // Todo use names to 
    public void playVoice(AudioClip voice)
    {
        if(voiceAudioSource != null)
        {
            Debug.Log("Playing Scream");
            voiceAudioSource.PlayOneShot(voice);
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogWarning("No Rigidbody attached to this object.");
            return;
        }
        if (collision.gameObject.CompareTag("Rail"))
        {
            StartGrinding(collision.gameObject);
            return;
        }
        ContactPoint[] contactPoints = new ContactPoint[collision.contactCount];
        collision.GetContacts(contactPoints);

        for (int i = 0; i < contactPoints.Length; i++)
        {   
            if (deathCollider != null && contactPoints[i].thisCollider.gameObject == deathCollider)
            {
                float impactForce = collision.relativeVelocity.magnitude;

                if(impactForce > lethalImpactThreshold)
                {
                    if(deathParticleEffect != null)
                    {
                        GameObject deathEffect = Instantiate(deathParticleEffect, contactPoints[i].point, Quaternion.identity);
                        Destroy(deathEffect, 1.0f);
                    }


                    if (hurtFxs.Length > 0)
                    {
                        int voiceLine = UnityEngine.Random.Range(0, hurtFxs.Length);
                        playVoice(hurtFxs[voiceLine]);
                    }

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

        Gizmos.DrawSphere(pogoCastStart + leanChild.transform.up * (-pogoRayCastLength + pogoCastRadius), pogoCastRadius);

        // Draw the pogo stick center
        Gizmos.DrawSphere(pogoStick.transform.position + leanChild.transform.rotation * flipAxisOffset, 0.3f);
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
    void StartGrinding(GameObject railObject)
    {

        if (isGrinding)
            return;

        isGrinding = true;
        currentRailScript = railObject.GetComponent<RailScript>();
        if (currentRailScript == null)
        {
            Debug.LogError("RailScript not found on the rail object.");
            isGrinding = false;
            return;
        }

        Vector3 splinePoint;
        float normalizedTime = currentRailScript.CalculateTargetRailPoint(transform.position, out splinePoint);
        grindElapsedTime = normalizedTime;

        float3 pos, forward, up;
        SplineUtility.Evaluate(currentRailScript.railSpline.Spline, normalizedTime, out pos, out forward, out up);
        currentRailScript.CalculateDirection(forward, transform.forward);
        normalDir = currentRailScript.normalDir;

        transform.position = splinePoint + (transform.up * heightOffset);

        rb.isKinematic = true;
        SetPlayerCollidersTrigger(true);
    }

    void MoveAlongRail()
    {
    if (currentRailScript == null)
        return;

    float deltaProgress = (grindSpeed / currentRailScript.totalSplineLength) * Time.deltaTime;
    grindElapsedTime += normalDir ? deltaProgress : -deltaProgress;

    float progress = grindElapsedTime;

    // For debugging: check that progress doesn't end early
    // Debug.Log($"Progress: {progress}, GrindElapsedTime: {grindElapsedTime}, DeltaProgress: {deltaProgress}");

    if (progress < -0.01f || progress > 1.01f)
    {
        EndGrinding();
        
        return;
    }

        float3 pos, tangent, up;
        float3 nextPosFloat, nextTan, nextUp;

        // Evaluate current position on the spline
        SplineUtility.Evaluate(currentRailScript.railSpline.Spline, progress, out pos, out tangent, out up);

        float nextProgress = progress + (normalDir ? deltaProgress : -deltaProgress);
        SplineUtility.Evaluate(currentRailScript.railSpline.Spline, nextProgress, out nextPosFloat, out nextTan, out nextUp);

        Vector3 worldPos = currentRailScript.LocalToWorldConversion(pos);
        Vector3 nextPos = currentRailScript.LocalToWorldConversion(nextPosFloat);

        transform.position = worldPos + (transform.up * heightOffset);

        //Quaternion targetRotation = Quaternion.LookRotation(nextPos - worldPos, up);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, lerpSpeed * Time.deltaTime);
    }

void EndGrinding()
{
    isGrinding = false;
    currentRailScript = null;
    rb.isKinematic = false;

    // Experiment with this and maybe remove in the future
    Physics.IgnoreLayerCollision(3, 4, true);
    transform.position += transform.up * 0.5f;


    rb.AddForce(leanChild.transform.up * 50f, ForceMode.Impulse);
    pogoStick.transform.localScale = Vector3.one;


    reEnableCollidersPending = true;
    colliderReEnableTimer = 0f; 
}

void SetPlayerCollidersTrigger(bool isTrigger)
{
    Collider[] colliders = GetComponentsInChildren<Collider>();
    foreach (Collider col in colliders)
    {
        col.isTrigger = isTrigger;
    }
}
}
