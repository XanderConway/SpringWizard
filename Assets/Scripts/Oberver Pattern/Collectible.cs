using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CollectibleData
{
    public string collectibleId;
    public int points;
}

public class Collectible : MonoBehaviour
{

    public CollectibleData collectible;

    private UnityEvent<CollectibleData> collectibleEvent;

    public AudioClip collectionNoise;
    private AudioSource audioSource;

    void OnEnable()
    {
        collectibleEvent = new UnityEvent<CollectibleData>();
    }

    private void Start()
    {
        audioSource = gameObject.GetComponentInParent<AudioSource>();
    }


    public UnityEvent<CollectibleData> getEvent()
    {
        return collectibleEvent;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger has the player tag
        if (other.CompareTag("Collector"))
        {
            // Logic for what happens when the player collects the item
            collectibleEvent.Invoke(collectible);
            if (audioSource != null)
            {
                audioSource.PlayOneShot(collectionNoise);
            }
            Destroy(gameObject);
        }
    }


}
