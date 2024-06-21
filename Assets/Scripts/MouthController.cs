using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthController : MonoBehaviour
{
    private Animator animator;
    private bool speaking;

    void Start()
    {
        // Get the Animator component attached to the Plane GameObject
        animator = GetComponent<Animator>();
        
        // Initially, the character is not speaking
        speaking = false;
    }

    // Function to play the speaking animation
    public void StartSpeakingAnimation()
    {
        // Set the flag to true to indicate that the character is speaking
        speaking = true;
    }

    // Function to stop the speaking animation
    public void StopSpeakingAnimation()
    {
        // Set the flag to true to indicate that the character is speaking
        speaking = false;
    }

    void Update()
    {
        // Check if the character is speaking
        if (speaking)
        {
            // Trigger the "SpeakAnimation" in the Animator
            animator.SetBool("isSpeaking", true);
        }
        else
        {
            // Trigger the "IdleAnimation" in the Animator
            animator.SetBool("isSpeaking", false);
        }
    }
}
