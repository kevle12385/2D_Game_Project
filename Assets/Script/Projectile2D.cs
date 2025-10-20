using UnityEngine;

public class Projectile2D : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;
    public int damage = 1;

    Rigidbody2D rb;
    Collider2D myCol;
    Transform owner;

    // Called right after instantiation by the shooter
    public void SetOwner(Transform ownerRoot)
    {
        owner = ownerRoot;

        if (!myCol) myCol = GetComponent<Collider2D>();
        if (!myCol || !owner) return;

        // Ignore collisions with all colliders on the owner
        var ownerCols = owner.GetComponentsInChildren<Collider2D>(true);
        foreach (var oc in ownerCols)
            if (oc) Physics2D.IgnoreCollision(myCol, oc, true);
    }

    // Launch the projectile in a direction
    public void Launch(Vector2 dir)
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir.normalized * speed;
        Invoke(nameof(Die), lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Never hit the owner
        if (owner && other.transform.IsChildOf(owner)) return;

        // Ignore other projectiles
        if (other.GetComponent<Projectile2D>() != null) return;

        // Deal damage
        var hp = other.GetComponentInParent<Health2D>();
        if (hp != null) hp.TakeDamage(damage);

        Die();
    }

    void Die() => Destroy(gameObject);
}
