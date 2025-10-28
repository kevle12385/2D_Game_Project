using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem; // new input system
#endif

public class DoorSceneChanger : MonoBehaviour
{
    [Header("Target")]
    public string sceneToLoad; // name of scene to load
    public string spawnId = "default"; // spawnId  set for PlayerSpawnManager

    [Header("Interaction")] 
    public KeyCode interactKey = KeyCode.E;      // old input system fallback
    public bool useNewInputSystem = true;        // set true if you use the new Input System
    public float cooldownSeconds = 0.4f; // cooldown between scene changes

    bool playerInside; //true when player is inside trigger zone
    static float globalCooldownUntil = 0f; // static cooldown timer shared by all doors

    void OnTriggerEnter2D(Collider2D other) // when player collider enters this trigger zone
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
    }

    void OnTriggerStay2D(Collider2D other) // when player collider stays inside this trigger zone
    {
        if (other.CompareTag("Player")) playerInside = true; // covers spawn already inside
    }

    void OnTriggerExit2D(Collider2D other) // when player collider exits this trigger zone
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    void Update() 
{
    var col = GetComponent<Collider2D>();
    var player = GameObject.FindGameObjectWithTag("Player");
    if (!col || !player) return;

    // treat “inside” as true whenever colliders overlap
    playerInside = col.IsTouching(player.GetComponent<Collider2D>());

    if (!playerInside) return; // only proceed if player is inside trigger zone
    if (Time.time < globalCooldownUntil) return; // still in cooldown period, 

#if ENABLE_INPUT_SYSTEM
    bool pressed = (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame); // if E key was pressed this frame
#else
    bool pressed = Input.GetKeyDown(KeyCode.E); // if E key was pressed this frame
#endif

    if (pressed) // if interaction key was pressed
    {
        PlayerSpawnManager.NextSpawnId = spawnId; // set next spawn ID for PlayerSpawnManager
        globalCooldownUntil = Time.time + 0.4f; // set global cooldown timer
        SceneManager.LoadScene(sceneToLoad); // load the target scene
    }
}

}
