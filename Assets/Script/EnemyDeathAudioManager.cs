using UnityEngine;

public class EnemyDeathAudioManager : MonoBehaviour
{
    private static EnemyDeathAudioManager instance;
    private AudioSource audioSource;

    [Header("Enemy Death Sound")]
    public AudioClip enemyDeathClip; // sound to play when an enemy dies
    [Range(0f, 1f)] public float volume = 1f; // volume for the death sound

    void Awake()
    {
        // Singleton pattern so there’s only one persistent instance
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        instance = this;  // assign this object as actice instance
        audioSource = GetComponent<AudioSource>(); // get AudioSource component
    }

    // Static method any enemy can call when it dies
    public static void PlayEnemyDeathSound(Vector3 position)
    {
        if (instance != null && instance.enemyDeathClip != null)
        {
            instance.audioSource.transform.position = position; // move to death position
            instance.audioSource.PlayOneShot(instance.enemyDeathClip, instance.volume); // play sound
        }
    }
}
