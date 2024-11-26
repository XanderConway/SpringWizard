using UnityEngine;

public class Springboard : MonoBehaviour
{
    public Vector3 springForce = Vector3.zero;
    private Animator animController;

    public AudioClip bounceSound;
    private AudioSource audioSource;

    private void Start()
    {
        animController = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PogoControls pogoControls = other.gameObject.GetComponentInParent<PogoControls>();

        animController.SetTrigger("Pop");
        
        if (pogoControls != null && !pogoControls.IsDead())
        {
            Debug.Log("Adding Force");
            Rigidbody rb = pogoControls.gameObject.GetComponent<Rigidbody>();
            pogoControls.ResetJumpState();
            rb.velocity = new Vector3(0, 1);
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(transform.rotation * springForce, ForceMode.VelocityChange);
            //rb.AddForce(springForce);
            audioSource.PlayOneShot(bounceSound);

            pogoControls.NotifyTrickObservers(PlayerTricks.springboard);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + (transform.rotation *  springForce).normalized * 100);
    }
}