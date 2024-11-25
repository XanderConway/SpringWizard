using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip fallScream;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Death triggered");
        PogoControls playerControls = other.attachedRigidbody.gameObject.GetComponent<PogoControls>();

        if(playerControls != null )
        {
            playerControls.setDead(true, true);

            if (fallScream != null)
            {
                playerControls.playVoice(fallScream);
            }
        }
        
    }
}
