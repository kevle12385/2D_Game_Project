using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    public Transform handSocket;   // child on player where gun attaches
    public Weapon2D currentWeapon;

    float moveX;
    bool firingHeld;

    void Update()
    {
        // update facing by movement (optional)
        if (moveX != 0)
        {
            var s = transform.localScale;
            s.x = Mathf.Abs(s.x) * (moveX > 0 ? 1 : -1);
            transform.localScale = s;
        }

        if (currentWeapon && firingHeld && currentWeapon.automatic)
        {
            Vector2 dir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
            currentWeapon.TryShoot(dir);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveX = ctx.ReadValue<Vector2>().x;
        
        Debug.Log("moveX = " + moveX);
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
        else if (ctx.canceled) firingHeld = false;
    }

    void OnTriggerEnter2D(Collider2D other)
{
    if (currentWeapon) return; // already holding something

    // Works whether the trigger is on the weapon root or a child
    Weapon2D w = other.GetComponentInParent<Weapon2D>();
    if (w != null)
    {
        Equip(w);
        // Debug.Log("Picked up: " + w.name);
    }
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
