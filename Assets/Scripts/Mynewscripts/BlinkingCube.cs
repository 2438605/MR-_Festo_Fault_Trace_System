using UnityEngine;

public class BlinkingCube : MonoBehaviour
{
    public GameObject triggerZone;         // Drag the TriggerZone here
    public AudioSource triggerAudioSource; // Drag the AudioSource from TriggerZone
    private Animator animator;             // Animator component reference

    private void Start()
    {
        animator = GetComponent<Animator>(); // Get Animator Component

        if (triggerZone == null)
        {
            Debug.LogError("TriggerZone is not assigned in the Inspector!");
        }

        if (triggerAudioSource == null)
        {
            Debug.LogError("AudioSource is not assigned in the Inspector!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crate"))
        {
            animator.SetBool("isBlinking", true); // Start Animation
            PlayFaultSound(); // Play Audio
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Crate"))
        {
            animator.SetBool("isBlinking", false); // Stop Animation
        }
    }

    private void PlayFaultSound()
    {
        if (triggerAudioSource != null && !triggerAudioSource.isPlaying)
        {
            triggerAudioSource.Play();
        }
    }
}
