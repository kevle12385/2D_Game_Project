

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerSpawnManager : MonoBehaviour
{
    public static string NextSpawnId = "default"; // the spawn ID to use on next scene load

    static PlayerSpawnManager _inst; // singleton instance
    void Awake()
    {
        if (_inst && _inst != this) { Destroy(gameObject); return; } // prevent duplicates PlayerSpawnManager objects
        _inst = this;
        DontDestroyOnLoad(gameObject); //PlayerSpawnManager survives scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;  // on scene load call OnSceneLoaded(), to position player at the spawn point specified by NextSpawnId
    }

    void OnSceneLoaded(Scene s, LoadSceneMode m) // when a new scene is loaded, position the player at the  spawn point specified by NextSpawnId
    {
        // Find the persistent player (root object that survives scene loads)

        var player = Object.FindFirstObjectByType<PersistentPlayer>()?.transform; 
        if (!player) return; // no persistent player found, do nothing

        SpawnPoint target = null; 
        // Find all spawn points in the scene and pick the one whose id matches NextSpawnId
        var points = Object.FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        foreach (var sp in points) if (sp.id == NextSpawnId) { target = sp; break; }

        //  if no matching id, use the first spawn point (if any)
        if (!target && points.Length > 0) target = points[0];
        if (!target) return;

        // Move player to the spawn point position
        var p = target.transform.position;
        player.position = new Vector3(p.x, p.y, 0f);

        // Reset physics so the player doesn't carry momentum between scenes
        var rb = player.GetComponent<Rigidbody2D>();
        if (rb) { rb.linearVelocity = Vector2.zero; rb.angularVelocity = 0f; }

     
    }

  
}
