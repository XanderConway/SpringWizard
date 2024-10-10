using UnityEngine;

public class Springboard : MonoBehaviour
{
    public float springForce = 20f;  
    
    private void OnCollisionEnter(Collision collision)
    {
        PogoControls pogoControls = collision.gameObject.GetComponent<PogoControls>();
        
        if (pogoControls != null)
        {
            pogoControls.ApplySpringboardForce(springForce);
        }
    }
}