using UnityEngine;

public class Weapon2D : MonoBehaviour
{
    public Transform firePoint;             // muzzle Transform
    public Projectile2D projectilePrefab;   // projectile prefab reference
    
    public float fireRate = 5f;
    public bool automatic = true;

    float cd;

    public void TryShoot(Vector2 dir)
    {
        cd -= Time.deltaTime;
        if (cd <= 0f)
        {
            Shoot(dir);
            cd = 1f / Mathf.Max(0.01f, fireRate);
        }
    }

    public void ShootOnce(Vector2 dir)
    {
        if (cd <= 0f)
        {
            Shoot(dir);
            cd = 1f / Mathf.Max(0.01f, fireRate);
        }
    }

    void Shoot(Vector2 dir)
    {
        if (!projectilePrefab || !firePoint) return;

        // Spawn projectile
        var p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Tell projectile who fired it so it ignores our colliders
        var owner = transform.root != null ? transform.root : transform;
        p.SetOwner(owner);

        // If no direction given, use firePoint facing
        if (dir == Vector2.zero)
            dir = firePoint.right;

        // Launch projectile
        p.Launch(dir);
        
    }
}
