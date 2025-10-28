using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public string id = "default"; //  A unique identifier string for this spawn point.


    // Editor gizmo so you can see it
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(0.6f, 0.6f, 0.1f));
#if UNITY_EDITOR
        UnityEditor.Handles.Label(transform.position + Vector3.up * 0.4f, id);
#endif
    }
}
