using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public Transform pogo_stick;
    public Transform magic_spawn;

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
    void Update()
    {
        handleControls();

        if (!isGrounded)
        {
            rotationEnd = pogo_stick.up;
            float angle = Vector3.Angle(rotationStart, rotationEnd);
            rotationDifference += angle;
            rotationStart = rotationEnd;
        }
    }

    void handleControls()
    {
        float forward_input = Input.GetAxis("Vertical");
        float side_input = Input.GetAxis("Horizontal");
        Quaternion rotation = Quaternion.AngleAxis(forward_input * rotationSpeed * Time.deltaTime, transform.right);
        pogo_stick.rotation = rotation * pogo_stick.rotation;

        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = Vector3.zero;
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        rb.AddForce(pogo_stick.transform.up * jumpForce, ForceMode.Impulse);

        isGrounded = true;
        Debug.Log("rotationDifference: " + rotationDifference);
        if (rotationDifference > rotationThreshold)
        {
            Debug.Log("Trick!  " + rotationDifference);
            TrickDetected();
            rotationDifference = 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {

        isGrounded = false;
    }

    void TrickDetected()
    {
        GameObject effect = Instantiate(trickMagic, magic_spawn.transform.position, Quaternion.identity);
        Destroy(effect, 1);
        //destroy the magic after 1 second
    }
}
