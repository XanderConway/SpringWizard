using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public float gravityMultiplier;
    public Collider bounceCollider;
    public Rigidbody rb;
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
        transform.rotation *= Quaternion.AngleAxis(forward_input * rotationSpeed * Time.deltaTime, transform.right);
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        Debug.Log("Collided!");

    }
}
