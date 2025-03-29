using UnityEngine;

public class TriggerZoneAudio : MonoBehaviour
{
    public AudioSource triggerAudioSource; // Drag the AudioSource from Inspector

    private void Start()
    {
        // Ensure the AudioSource is assigned
        if (triggerAudioSource == null)
        {
            Debug.LogError("AudioSource is NOT assigned in the Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crate")) // Check if the Crate enters the trigger zone
        {
            Debug.Log("Crate entered trigger zone."); // Debug message
            PlayFaultSound();
        }
    }

    private void PlayFaultSound()
    {
        if (triggerAudioSource != null && !triggerAudioSource.isPlaying)
        {
            Debug.Log("Playing fault sound..."); // Debug message
            triggerAudioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource is missing or already playing!");
        }
    }
}
