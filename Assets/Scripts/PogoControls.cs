using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float rotationSpeed = 360f;
    public float leanSpeed = 360f;
    public float maxLeanAngle = 15;

    private float currentLeanAngle = 0;

    public float jumpForce = 1.0f;

    public float pogoRayCastLength = 10f;
    public Vector3 pogoRayCastOffset = Vector3.zero;
    bool enteredPogoRange = false;

    public float compressTime = 0.4f;
    private float compressTimer = 0;
    private bool compressing = false;


    public Transform pogoStick; // Will flip around it's side axis
    public Transform leanChild; // Will rotate around forward axis, should be the parent of pogostick

    public Transform magicSpawn;

    private float gravityMultiplier;

    private Collider bounceCollider;
    private Rigidbody rb;

    // trick detection
    public bool isGrounded;
    public Vector3 rotationStart;
    public Vector3 rotationEnd;
    public float rotationDifference;
    public float rotationThreshold = 360.0f;

    // a magic to be spwaned when a trick is detected (sparkles, etc)
    public GameObject trickMagic;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bounceCollider = GetComponent<Collider>();

  
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        detectJumping();

        //if (!isGrounded)
        //{
        //    rotationEnd = pogo_stick.up;
        //    float angle = Vector3.Angle(rotationStart, rotationEnd);
        //    rotationDifference += angle;
        //    rotationStart = rotationEnd;
        //}
    }

    private void Update()
    {
        handleControls();
    }

    void detectJumping()
    {
        Vector3 pogoCastStart = leanChild.transform.position + leanChild.transform.rotation * pogoRayCastOffset;
        Vector3 pogoCastEnd = pogoCastStart + leanChild.transform.rotation * leanChild.transform.up * (-pogoRayCastLength);
        RaycastHit hit;
        LayerMask layerMask = ~0; // Collide with every layer

        if (Physics.Raycast(pogoCastStart, -1 * leanChild.transform.up, out hit, pogoRayCastLength, layerMask))
        {
            // Compress the spring if there is ground below us and we are moving downwards
            if (!compressing && rb.velocity.y <= 0)
            {
                compressing = true;
                Debug.Log("Activating compression" + hit.collider.gameObject.name);

                //rb.AddForce(pogo_stick.transform.up * jumpForce, ForceMode.Impulse);
            }
        }

        if (compressing)
        {
            compressTimer += Time.deltaTime;
        }

        if (compressTimer > compressTime)
        {
            compressTimer = 0;
            compressing = false;
            Jump();
            Debug.Log("Jumping");

        }
    }

    void handleControls()
    {
        float forwardInput = Input.GetAxis("Vertical");

        float sideInput = Input.GetAxis("Horizontal");

        Quaternion xRotation = Quaternion.AngleAxis(forwardInput * rotationSpeed * Time.deltaTime, transform.right);
        pogoStick.rotation = xRotation * pogoStick.rotation;

        currentLeanAngle -= sideInput * leanSpeed * Time.deltaTime; // Should be subtracted

        currentLeanAngle = Mathf.Clamp(currentLeanAngle, -maxLeanAngle, maxLeanAngle);
        leanChild.localRotation = Quaternion.AngleAxis(currentLeanAngle, Vector3.forward);
        

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = Vector3.zero;
        }
    }

    private void Jump()
    {
        rb.AddForce(leanChild.transform.up * jumpForce, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Duplicated from control handling
        Vector3 pogoCastStart = leanChild.transform.position + leanChild.transform.rotation * pogoRayCastOffset;
        Vector3 pogoCastEnd = pogoCastStart + leanChild.transform.up * (-pogoRayCastLength);
        Gizmos.DrawLine(pogoCastStart, pogoCastEnd);
    }

    void TrickDetected()
    {
        GameObject effect = Instantiate(trickMagic, magicSpawn.transform.position, Quaternion.identity);
        Destroy(effect, 1);
        //destroy the magic after 1 second
    }
}
