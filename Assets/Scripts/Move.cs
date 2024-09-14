using UnityEngine;

public class Move : MonoBehaviour
{

    [SerializeField, Min(0)] float moveSpeed = 5f, jumpHeight = 200f;
    [SerializeField, Range(0, 5)] int maxJump = 2;
    Rigidbody rb;
    int jumped;

    void Start () {
        rb = GetComponent<Rigidbody>();
        jumped = 0;
    }

    void Update () {
        float scale = moveSpeed * Time.deltaTime;
        transform.localPosition += transform.forward * scale * Input.GetAxis("Vertical");
        transform.localPosition += transform.right * scale * Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && jumped++ < maxJump) {
            rb.AddForce(Vector3.up * jumpHeight);
        }
    }

    void OnCollisionEnter () {
        jumped = 0;
    }
}
