using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * POGO STICK CONTROLS
 * WS -> Rotate forward and backwards
 * AD -> Rotate side to side
 * SPACE -> Hold space when on the ground to compress the spring longer and jump higher
 * MOUSE -> Pan to change the look direction
 * Q -> Perform trick 1 (Needs to be performed with a flip for effect)
 * E -> Perform trick 2 (Needs to be performed with a flip for effect)
 */

public class PogoControls : MonoBehaviour
{

    // Rotation parameters
    public float rotationSpeed = 360f;
    public float leanSpeed = 360f;
    public float maxLeanAngle = 15;

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

    // Parameters for jumping
    public float baseJumpForce = 1.0f;
    public float maxHeldJumpForce = 3.0f;
    public float compressTime = 0.4f;
    public float maxJumpHoldTime = 1.0f;

    public GameObject mainPogoBody;
    public float springLength = 1.0f;
    public float springMaxCompression = 0.003f;
    private Vector3 pogoBodyHeightOffGround;

    // Head collision Checker
    public Vector3 headOffset;
    public float headRadius;

    public AnimationCurve bounceScale;
    private float groundedTimer = 0;
    private bool grounded = false;
    private bool holdingJump = false;
    private float bonusJumpForce = 0;

    // Transforms used for rotations
    public Transform pogoStick; // Will flip around it's side axis
    public Transform leanChild; // Will rotate around forward axis, should be the child of pogostick
    private Rigidbody rb;
    public Material wizardMaterial;


    // particle effects
    public GameObject fireEffect;
    public GameObject iceEffect;
    public GameObject earthEffect;
    public GameObject airEffect;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pogoBodyHeightOffGround = mainPogoBody.transform.localPosition;
    }

    void FixedUpdate()
    {
        detectJumping();
        countFlips();
    }

    private void Update()
    {
        handleControls();
    }

    // Called when the pogo stick hits the ground
    void groundedEvent()
    {
        GameObject effect = null;
        // TODO: Robby call your spell effects here
        if (flipType > 0)
        {
            if (currTrick > 0)
            {
                Debug.Log("ICE");

            }
            else if (currTrick < 0)
            {
                effect = Instantiate(earthEffect, transform.position, Quaternion.identity);
                Debug.Log("EARTH");
            }
        }
        else if (flipType < 0)
        {
            {
                if (currTrick > 0)
                {
                    Debug.Log("FIRE");
                }
                else if (currTrick < 0)
                {
                    effect = Instantiate(airEffect, transform.position, Quaternion.identity);
                    Debug.Log("AIR");
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
    }

    float groundSpeed = 0;
    void detectJumping()
    {
        Vector3 pogoCastStart = leanChild.transform.position + leanChild.transform.rotation * pogoRayCastOffset;
        Vector3 pogoCastEnd = pogoCastStart + leanChild.transform.rotation * leanChild.transform.up * (-pogoRayCastLength);
        RaycastHit hit;
        LayerMask layerMask = ~0; // Collide with every layer

        // TODO: Robby, you can remove water layers from this layer mask, and only add them when the ice trick is performed,
        // so that we only jump off water when the trick happens. 
        
        //remove water layer from layer mask
        
        layerMask = ~LayerMask.GetMask("Water");
        
         if (flipType > 0)
        {
            if (currTrick > 0)
            {
                layerMask = ~0;
                Physics.IgnoreLayerCollision(3, 4, false);
                Debug.Log("ICE");
            }
        }

        if (Physics.Raycast(pogoCastStart, -1 * leanChild.transform.up, out hit, pogoRayCastLength, layerMask))
        {
            // Compress the spring if there is ground below us and we are moving downwards
            if (!grounded && rb.velocity.y <= 0)
            {
                grounded = true;
                groundSpeed = rb.velocity.y;
                groundedEvent();
            }
        }
        //else if ice trick is triggered
        

        if (grounded)
        {
            groundedTimer += Time.deltaTime;

            // Current compression is inital_velocity * cos(time)
            float maxCompression = groundSpeed * springMaxCompression;
            float bounceAmount = maxCompression * Mathf.Cos(groundedTimer / compressTime * 2 * Mathf.PI) + 0.5f;
            
            // The spring is fully compressed, begin decompressing this usually feels bad
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
            if(holdingJump && groundedTimer < maxJumpHoldTime)
            {
                bonusJumpForce = ((groundedTimer - compressTime) / maxJumpHoldTime) * maxHeldJumpForce;
            }
            else
            {
                Jump(baseJumpForce + bonusJumpForce + fireJumpBoost);

                groundedTimer = 0;
                bonusJumpForce = 0;
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

    void handleControls()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float sideInput = Input.GetAxis("Horizontal");

        float flipAngle = forwardInput * rotationSpeed * Time.deltaTime;

        // Change flip speed based on the angle, we want to move faster when upside down
        float dotProduct = Vector3.Dot(pogoStick.up.normalized, transform.up.normalized);
        flipAngle *= flipSpeedMultiplier.Evaluate(dotProduct);

        // Rotate around the foot of the pogo stick when grounded, and the center when in the air
        if (grounded)
        {
            pogoStick.Rotate(transform.right, flipAngle, Space.World);
        } else
        {
            Vector3 rotationCenter = leanChild.transform.position + leanChild.transform.rotation * flipAxisOffset;
            pogoStick.RotateAround(rotationCenter, transform.right, flipAngle);
        }

        currentLeanAngle -= sideInput * leanSpeed * Time.deltaTime;

        // If there is no input, move the lean rotation back to 0
        if(Mathf.Abs(sideInput) < 0.1)
        {
            float leanAdjustment = leanSpeed * Time.deltaTime * 0.8f;
            currentLeanAngle -= Mathf.Sign(currentLeanAngle) * Mathf.Min(leanAdjustment, Mathf.Abs(currentLeanAngle));
        }

        currentLeanAngle = Mathf.Clamp(currentLeanAngle, -maxLeanAngle, maxLeanAngle);
        leanChild.localRotation = Quaternion.AngleAxis(currentLeanAngle, Vector3.forward);

        holdingJump = Input.GetKey(KeyCode.Space);


        // TEMPORARY RESET BUTTON
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = Vector3.zero;
        }

        // Currently just changing colour as a place holder for animations
        if(Input.GetKey(KeyCode.Q))
        {
            currTrick = 1;
        } else if(Input.GetKey(KeyCode.E))
        {
            currTrick = -1;
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
        Gizmos.DrawSphere(pogoStick.transform.position + pogoStick.transform.rotation * flipAxisOffset, 4);


        Gizmos.color = Color.blue;
    }
}
