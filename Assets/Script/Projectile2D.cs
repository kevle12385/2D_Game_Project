using UnityEngine;

public class Projectile2D : MonoBehaviour // a 2D projectile that will deal damage on contact
{
    public float speed = 20f; // projectile speed
    public float lifetime = 2f; // bullet lifetime before self-destruct
    public int damage = 1; // damage dealt on hit

    Rigidbody2D rb; 
    Collider2D myCol; //Collider attached to this projectile
    Transform owner;

    // Called right after instantiation by the shooter

    public void SetOwner(Transform ownerRoot) // ignore the collision with the player that fired it
    {
        owner = ownerRoot;

        if (!myCol) myCol = GetComponent<Collider2D>(); // get our own collider if not already
        // if (!myCol || !owner) return; // 

        // Ignore collisions with all colliders on the owner
        var ownerCols = owner.GetComponentsInChildren<Collider2D>(true); // collects all colliders on owner
        foreach (var collider_ in ownerCols) // for each collider on the owner
            if (collider_) Physics2D.IgnoreCollision(myCol, collider_, true); // ignore collision between projectile and owner collider
    }

    // Launch the projectile in a direction
    public void Launch(Vector2 dir)
    {
        if (!rb) rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir.normalized * speed; // set the velocity of the projectile rigidbody
        Invoke(nameof(Die), lifetime); // destroy the projectile after lifetime seconds
    }   // calls the function Die after lifetime seconds

    void OnTriggerEnter2D(Collider2D other) // When projectile's collider hits another collider
    {
        // Never hit the owner
        if (owner && other.transform.IsChildOf(owner)) return; // ignore owners colliders

        // Ignore other projectiles
        if (other.GetComponent<Projectile2D>() != null) return; // ignore other projectiles objects

        // Deal damage
        var hp = other.GetComponentInParent<Health2D>(); // get Health2D component from the object we hit
        if (hp != null) hp.TakeDamage(damage); // deal damage to that object if it has Health2D component

        Die(); // destroy the projectile on hit
    }

    void Die() => Destroy(gameObject); // Die() deletes the projectile 
}
