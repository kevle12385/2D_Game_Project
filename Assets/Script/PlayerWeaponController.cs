// using UnityEngine;
// using UnityEngine.InputSystem;

// public class PlayerWeaponController : MonoBehaviour
// {
//     public Transform handSocket;   // child on player where gun attaches
//     public Weapon2D currentWeapon;

//     float moveX;
//     bool firingHeld;

//     void Update()
//     {
//         // update facing by movement (optional)
//         if (moveX != 0)
//         {
//             var s = transform.localScale;
//             s.x = Mathf.Abs(s.x) * (moveX > 0 ? 1 : -1);
//             transform.localScale = s;
//         }

//         if (currentWeapon && firingHeld && currentWeapon.automatic)
//         {
//             Vector2 dir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
//             currentWeapon.TryShoot(dir);
//         }
//     }

//     public void OnMove(InputAction.CallbackContext ctx)
//     {
//         moveX = ctx.ReadValue<Vector2>().x;


//     }

//     public void OnFire(InputAction.CallbackContext ctx)
//     {
//         if (!currentWeapon) return;
//         if (ctx.performed)
//         {
//             firingHeld = true;
//             if (!currentWeapon.automatic)
//             {
//                 Vector2 dir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
//                 currentWeapon.ShootOnce(dir);
//             }
//         }
//         else if (ctx.canceled) firingHeld = false;
//     }

//     void OnTriggerEnter2D(Collider2D other)
// {
//     // if (currentWeapon) return; // already holding something

//         // Works whether the trigger is on the weapon root or a child
//         Weapon2D w = other.GetComponentInParent<Weapon2D>();

//     if (currentWeapon != null)
//     {
//         // Detach from hand
//         currentWeapon.transform.SetParent(null);

//         // Enable physics so it falls to the ground
//         var rb = currentWeapon.GetComponent<Rigidbody2D>();
//         if (rb)
//         {
//             rb.simulated = true;
//             rb.linearVelocity = Vector2.zero; // stop weird carry-over velocity
//         }

//         // Enable collider so it can be picked up again
//         var col = currentWeapon.GetComponent<Collider2D>();
//         if (col) col.enabled = true;

//         // Optional: add a small throw force so it drops away from the player
//         if (rb) rb.AddForce(Vector2.up * 3f + Vector2.right * transform.localScale.x * 2f, ForceMode2D.Impulse);
//     }

//     if (w != null)
//     {
//         Equip(w);
//         // Debug.Log("Picked up: " + w.name);
//     }
// }


//     public void Equip(Weapon2D w)
//     {
//         currentWeapon = w;
//         w.transform.SetParent(handSocket);
//         w.transform.localPosition = Vector3.zero;
//         w.transform.localRotation = Quaternion.identity;

//         var rb = w.GetComponent<Rigidbody2D>(); if (rb) rb.simulated = false;
//         var col = w.GetComponent<Collider2D>(); if (col) col.enabled = false;
//     }



// }

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform handSocket;   // where the weapon attaches to the player
    public Weapon2D currentWeapon;

    private Weapon2D nearbyWeapon; // the weapon you're standing near
    private float lastDropTime;    // used to prevent instant re-pickup
    bool firingHeld;
    float moveX;

    void Update()
    {
        // Flip player sprite based on movement
        if (moveX != 0)
        {
            var s = transform.localScale;
            s.x = Mathf.Abs(s.x) * (moveX > 0 ? 1 : -1);
            transform.localScale = s;
        }

        // Continuous firing for automatic weapons
        if (currentWeapon && firingHeld && currentWeapon.automatic)
        {
            Vector2 dir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
            currentWeapon.TryShoot(dir);
        }

        // 🟢 Press "1" to pick up weapon
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            TryPickupNearbyWeapon();
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveX = ctx.ReadValue<Vector2>().x;
    }

    public void OnFire(InputAction.CallbackContext ctx)
    {
        if (!currentWeapon) return;

        if (ctx.performed)
        {
            firingHeld = true;
            if (!currentWeapon.automatic)
            {
                Vector2 dir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
                currentWeapon.ShootOnce(dir);
            }
        }
        else if (ctx.canceled)
        {
            firingHeld = false;
        }
    }

    // When player enters weapon trigger, remember it
    void OnTriggerEnter2D(Collider2D other)
    {
        Weapon2D w = other.GetComponentInParent<Weapon2D>();
        if (w != null && w != currentWeapon)
        {
            nearbyWeapon = w;
        }
    }

    // When player leaves weapon trigger, clear reference
    void OnTriggerExit2D(Collider2D other)
    {
        Weapon2D w = other.GetComponentInParent<Weapon2D>();
        if (w != null && w == nearbyWeapon)
        {
            nearbyWeapon = null;
        }
    }

    // Pressing 1 tries to pick up the nearby weapon
    void TryPickupNearbyWeapon()
    {
        if (nearbyWeapon == null) return;
        if (Time.time - lastDropTime < 0.3f) return; // small cooldown

        // Drop current if holding one
        if (currentWeapon != null)
        {
            DropCurrentWeapon();
        }

        // Equip the new one
        Equip(nearbyWeapon);
        nearbyWeapon = null;
    }

    void DropCurrentWeapon()
    {
        currentWeapon.transform.SetParent(null);

        var rb = currentWeapon.GetComponent<Rigidbody2D>();
        var col = currentWeapon.GetComponent<Collider2D>();

        if (rb)
        {
            rb.simulated = true;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.AddForce(Vector2.up * 3f + (Vector2)transform.right * 2f, ForceMode2D.Impulse);
        }

        if (col) col.enabled = true;

        lastDropTime = Time.time;
        currentWeapon = null;
    }

    public void Equip(Weapon2D w)
    {
        currentWeapon = w;
        w.transform.SetParent(handSocket);
        w.transform.localPosition = Vector3.zero;
        w.transform.localRotation = Quaternion.identity;

        var rb = w.GetComponent<Rigidbody2D>(); if (rb) rb.simulated = false;
        var col = w.GetComponent<Collider2D>(); if (col) col.enabled = false;
    }
}
