using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public Transform pogo_stick;

    private float gravityMultiplier;
    private Collider bounceCollider;
    private Rigidbody rb;
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
    }

    void handleControls()
    {
        float forward_input = Input.GetAxis("Vertical");
        float side_input = Input.GetAxis("Horizontal");
        Quaternion rotation = Quaternion.AngleAxis(forward_input * rotationSpeed * Time.deltaTime, transform.right);
        pogo_stick.rotation = rotation * pogo_stick.rotation;

    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(pogo_stick.transform.up * jumpForce, ForceMode.Impulse);
    }
}
