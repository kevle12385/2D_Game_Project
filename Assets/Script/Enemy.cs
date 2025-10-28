

using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Rigidbody2D rb;
    private Transform player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;     // Don’t fall
        rb.bodyType = RigidbodyType2D.Kinematic;

        // 🔍 Automatically find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning("Enemy could not find a GameObject with tag 'Player'!");
    }

    void Update()
    {
        if (player == null) return;

        // Move directly toward player
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // Optional: flip sprite based on direction
        if (direction.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(direction.x), 1, 1);
    }
}
