using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Health2D playerHealth;
    public Camera deathCamera;

    void Start()
    {
        playerHealth.onDeath.AddListener(OnPlayerDeath);
    }

    void OnPlayerDeath()
    {
        deathCamera.enabled = true;
    }
}
