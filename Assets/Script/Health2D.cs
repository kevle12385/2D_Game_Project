using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // Needed for scene loading

public class Health2D : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 5; // set max health points for object
    [HideInInspector] public int currentHP; // current health points, hidden in inspector
    public bool destroyOnDeath = true; // boolean to whether  destroy object on death

    [Header("Invulnerability")] 
    public float hitInvulnTime = 0.1f; // set invulnerability time after being hit
    private float invulnTimer = 0f; // timer to track invulnerability duration

    [Header("Events")]
    public UnityEvent onDamaged; // Invoked whenever object takes damage
    public UnityEvent onDeath; // Invoked whenever object takes damage

    [Header("Death Settings")]
    public string deathSceneName = "DeathScene"; // scene to load on player death
    public Camera backupDeathCamera; // camera to activate on death

    [Header("Audio")]
    public AudioClip hurtClip;             // sound to play when hurt
    private AudioSource audioSource;       // internal reference

    public AudioClip deathClip;        // 🎵 plays when non-player dies




    void Awake() // called when the script instance is being loaded
    {
        // Always start full health for every object
        currentHP = maxHP; // set current health to max health
        audioSource = GetComponent<AudioSource>();  // get the player’s AudioSource

        if (CompareTag("Player")) // if this object is the player
        {

            DontDestroyOnLoad(gameObject); // make player persistent across scenes
        }
    }

    void Update()
    {
        if (invulnTimer > 0f) // if invulnerability timer is active
            invulnTimer -= Time.deltaTime; // reduce timer by time since last frame
    }

    public void TakeDamage(int amount) // function to apply damage to the object
    {
        if (invulnTimer > 0f || currentHP <= 0) // if invulnerable or already dead,
            return; // do nothing

        currentHP -= Mathf.Max(1, amount); // reduce current health by damage amount, minimum 1
        onDamaged?.Invoke(); // invoke damage event for UI updates, etc.
        invulnTimer = hitInvulnTime; // reset invulnerability timer

        if (hurtClip && audioSource)
        {
            audioSource.PlayOneShot(hurtClip);
        }

        if (currentHP <= 0) // if health drops to zero or below
        {
            currentHP = 0; // clamp health to zero
            onDeath?.Invoke(); // invoke death event
            if (!CompareTag("Player"))
            {
                EnemyDeathAudioManager.PlayEnemyDeathSound(transform.position);
            }





            if (CompareTag("Player")) // if this object is the player
            {
                ActivateDeathCamera(); // activate death camera
                
                if (!string.IsNullOrEmpty(deathSceneName)) // if a death scene is specified
                    SceneManager.LoadScene(deathSceneName); // load the death scene
            }

            






            if (destroyOnDeath) // if set to destroy on death, 
                Destroy(gameObject); // destroy this game object

        }
    }

    private void ActivateDeathCamera() // activates a backup camera on player death
    {
        // Try auto-find if not set
        if (backupDeathCamera == null) // if no backup camera assigned
        {
            var t = GameObject.FindGameObjectWithTag("DeathCam"); // find object with DeathCam tag
            if (t != null) backupDeathCamera = t.GetComponent<Camera>(); // get its Camera component
        }

        if (backupDeathCamera != null)
        {
            // Turn off player camera if assigned

            // Ensure only one AudioListener is active
            var activeListener = Object.FindFirstObjectByType<AudioListener>();
            if (activeListener != null) activeListener.enabled = false;
            
            // Ensure the backup camera has an AudioListener
            var backupListener = backupDeathCamera.GetComponent<AudioListener>();
            if (backupListener == null) backupListener = backupDeathCamera.gameObject.AddComponent<AudioListener>();

            // Activate the backup camera so the player the Death Sprite
            backupDeathCamera.gameObject.SetActive(true);
            backupDeathCamera.enabled = true;
        }
        
    }





    
    

}

