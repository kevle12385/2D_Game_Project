using UnityEngine;

public class PersistentPlayer : MonoBehaviour
{
    private static PersistentPlayer _instance;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);              // prevent duplicates if you come back to a scene
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);        // survive scene loads
    }
}
