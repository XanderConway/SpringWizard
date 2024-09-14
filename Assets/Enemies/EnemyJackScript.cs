using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform player;

    public float health = 100f;

    // Detection parameters
    public float detectionRange = 10f;
    public LayerMask whatIsPlayer;

    // Movement towards player
    // public float moveForce = 5f;

    // Bouncing parameters
    public float jumpForce = 5f;
    // public float groundCheckDistance = 1f;
    public LayerMask whatIsGround;
    private bool isGrounded;

    private Rigidbody rb;

    private void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        // GroundCheck();

        bool playerInRange = Physics.CheckSphere(transform.position, detectionRange, whatIsPlayer);

        if (playerInRange)
        {
            MoveTowardsPlayer();
        }

        if (isGrounded)
        {
            // Apply an upward force to make the cube bounce
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void MoveTowardsPlayer()
    {
        // Calculate direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Apply a force towards the player in X and Z axes
        Vector3 moveDirection = new Vector3(direction.x, 0, direction.z);
        rb.AddForce(moveDirection * moveForce, ForceMode.Force);
    }

    // private void GroundCheck()
    // {
    //     // Perform a raycast downwards to check if the cube is grounded
    //     isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, whatIsGround);
    // }


    // TODO: player damage
    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

}
