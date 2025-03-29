using UnityEngine;
using System.Collections;  // Add this line to use IEnumerator

public class StartCrateAnimation : MonoBehaviour
{
    public Animator crateAnimator;   // Assign the Animator component of the crate
    public string crateAnimationName = "Cube";  // Name of the animation to play
    public float animationSpeed = 1f;  // Speed of the animation playback

    private bool isAnimationPlaying = false;

    private void Start()
    {
        if (crateAnimator == null)
        {
            Debug.LogError("Animator is not assigned to the crate!");
        }
        else
        {
            // Set animation speed if necessary
            crateAnimator.speed = animationSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If Robotino enters the trigger zone
        if (other.CompareTag("Robotino") && !isAnimationPlaying)
        {
            Debug.Log("Robotino detected! Starting crate animation.");
            StartCrateAnimationPlay();
        }
    }

    private void StartCrateAnimationPlay()
    {
        isAnimationPlaying = true;

        // Play the crate animation
        crateAnimator.Play(crateAnimationName);

        // Optionally, you can reset the animation after it's completed
        StartCoroutine(ResetAnimation());
    }

    // Optional: Reset the animation once it's finished
    private IEnumerator ResetAnimation()
    {
        // Wait for the animation to finish (assuming it's not looping)
        yield return new WaitForSeconds(crateAnimator.GetCurrentAnimatorStateInfo(0).length);

        // Reset the animation back to idle or any other state
        crateAnimator.Play("Idle");  // Replace "Idle" with the default animation state
        isAnimationPlaying = false;
    }
}
