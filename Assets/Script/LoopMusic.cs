using UnityEngine;
using UnityEngine.InputSystem; // NEW input system

[RequireComponent(typeof(AudioSource))]
public class LoopMusic : MonoBehaviour

{


    private bool isMuted = false;
    private AudioSource audioSource;  // reference to the AudioSource


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;       // 🔁 makes it loop
        audioSource.playOnAwake = true; // starts automatically
        audioSource.Play();
    }


    void Update()
    {
        // Keyboard.current is null on platforms without a keyboard
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            isMuted = !isMuted; // toggle mute state
            audioSource.mute = isMuted; // apply mute state
        }
    }


}