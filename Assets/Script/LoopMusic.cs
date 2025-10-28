using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopMusic : MonoBehaviour
{
    void Start()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.loop = true;       // 🔁 makes it loop
        audio.playOnAwake = true; // starts automatically
        audio.Play();
    }
}