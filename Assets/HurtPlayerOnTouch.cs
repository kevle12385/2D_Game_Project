using UnityEngine;

public class HurtPlayerOnTouch : MonoBehaviour
{
    public int contactDamage = 1;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pickup")) return; // ignore pickups

        var hp = collision.gameObject.GetComponent<Health2D>();
        if (hp) hp.TakeDamage(contactDamage);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Pickup")) return;

        var hp = collider.GetComponent<Health2D>();
        if (hp) hp.TakeDamage(contactDamage);
    }
}
