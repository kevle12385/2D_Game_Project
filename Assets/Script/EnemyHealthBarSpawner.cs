using UnityEngine;

public class EnemyHealthTextSpawner : MonoBehaviour
{
    public EnemyHealthText textPrefab;

    void Start()
    {
        var text = Instantiate(textPrefab);
        text.target = GetComponent<Health2D>();
    }
}


