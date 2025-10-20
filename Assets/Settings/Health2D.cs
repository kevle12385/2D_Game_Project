using UnityEngine;
using UnityEngine.Events;

public class Health2D : MonoBehaviour
{
    [Header("Stats")]
    public int maxHP = 5;
    public int currentHP;   // optional custom attribute to lock edit
    public bool destroyOnDeath = true;

    [Header("Invulnerability")]
    public float hitInvulnTime = 0.1f; // brief grace so one bullet = one hit
    float invulnTimer = 0f;

    [Header("Events")]
    public UnityEvent onDamaged;
    public UnityEvent onDeath;

    void Awake() => currentHP = maxHP;

    void Update()
    {
        if (invulnTimer > 0f) invulnTimer -= Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (invulnTimer > 0f || currentHP <= 0) return;

        currentHP -= Mathf.Max(1, amount);
        onDamaged?.Invoke();
        invulnTimer = hitInvulnTime;

        if (currentHP <= 0)
        {
            onDeath?.Invoke();
            if (destroyOnDeath) Destroy(gameObject);
        }
    }

    // optional: knockback
    public void TakeHit(int amount, Vector2 force, Rigidbody2D rb = null)
    {
        TakeDamage(amount);
        if (rb != null && amount > 0) rb.AddForce(force, ForceMode2D.Impulse);
    }
}
