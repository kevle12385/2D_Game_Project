using UnityEngine;

public class Weapon2D : MonoBehaviour
{
    public Transform firePoint;             // where bullets spawn
    public Projectile2D projectilePrefab;   // projectile prefab reference, the object that will be spawned
    
    public float fireRate = 1f; // how fast we shoot
    public bool automatic = true; // hold to fire if true


    float cd; // cooldown timer


    public void TryShoot(Vector2 dir) // spawns a projectile in the dir direction
    {
        cd -= Time.deltaTime; // reduce the cooldown timer since the last frame
        
        if (cd <= 0f) // if coodldown is over, we can call shoot function
        {
            Shoot(dir); // function to shoot in the dir direction
            cd = 1f / Mathf.Max(0.01f, fireRate); // resetting the cooldwn by the fire rate
        }
    }

    public void ShootOnce(Vector2 dir) // if you only want to shoot once 
    {
        if (cd <= 0f)
        {
            Shoot(dir); // Shoot
            cd = 1f / Mathf.Max(0.01f, fireRate); //reset  cooldown
        }
    }

    void Shoot(Vector2 dir) //function to shoot in the dir direction
    {
        if (!projectilePrefab || !firePoint) return; // make sure we have a projectile and firepoint assigned

        // Spawn projectile
        var p = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Tell projectile who fired it so it ignores our colliders
        var owner = transform.root != null ? transform.root : transform;
        p.SetOwner(owner); // set owner is a function in Projectile2D.cs to set the owner of the projectile
        // so the bullet does not damage the player who fired it

        // If no direction given, use firePoint facing
        if (dir == Vector2.zero)
            dir = firePoint.right;

        // Launch projectile
        p.Launch(dir); // launch is a function in Projectile2D.cs to launch the projectile in the given direction
        
    }
}
