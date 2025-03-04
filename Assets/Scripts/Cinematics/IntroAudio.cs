using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAudio : MonoBehaviour
{
    // Array of audio clips
    public AudioClip[] audioClips;

    // Reference to the AudioSource
    public AudioSource audioSource;

    void Start()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // Function to switch the clip by passing an integer index
    public void SwitchAudioClip(int index)
    {
        // Check if the index is valid
        if (index >= 0 && index < audioClips.Length)
        {
            // Switch the audio clip
            audioSource.clip = audioClips[index];
            audioSource.Play(); // Optionally play the new clip immediately
        }
        else
        {
            Debug.LogWarning("Invalid index. Please provide a valid index.");
        }
    }

}
