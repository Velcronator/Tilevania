using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletLifeTime = 2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        Destroy(gameObject, bulletLifeTime);
    }

    public void Initialize(Vector2 direction, float speed)
    {
        rb.velocity = direction * speed;

        // if the bullet is moving to the left, flip the sprite using SpriteRenderer
        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Bullet OnTriggerEnter2D with " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("OnTriggerEnter2D Enemy hit!");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Bullet collided with " + collision.gameObject.name);
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
