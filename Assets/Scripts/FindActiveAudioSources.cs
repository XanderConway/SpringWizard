using UnityEngine;

public class FindActiveAudioSources : MonoBehaviour
{
    void Start()
    {
        // Get all AudioSource components in the scene
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Iterate through all AudioSources and check if they are playing
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                // Output the name of the GameObject that is playing sound
                Debug.Log("Object making noise: " + audioSource.gameObject.name);
            }
        }
    }
}
