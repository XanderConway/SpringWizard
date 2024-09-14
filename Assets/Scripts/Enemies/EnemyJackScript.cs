using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform player;

    public float health = 100f;

    // Detection parameters
    public float detectionRange = 10f;
    public LayerMask whatIsPlayer;

    // Movement towards player
    public float moveForce = 10f;

    // Bouncing parameters
    public float jumpForce = 5f;
    // public float groundCheckDistance = 1f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    private Rigidbody rb;

    private Animator animator;
    private bool isJumping = false;

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        isJumping = false;
        bool playerInRange = Physics.CheckSphere(transform.position, detectionRange, whatIsPlayer);

        if (playerInRange)
        {
            MoveTowardsPlayer();
        }

        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = true;
        }
    }

private void MoveTowardsPlayer()
{

    Vector3 direction = (player.position - transform.position).normalized;

    float moveSpeed = 5f; 
    Vector3 movement = direction * moveSpeed * Time.deltaTime;


    rb.MovePosition(transform.position + movement);
}

    // TODO: player health
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

}
