using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;

    private BoxCollider2D reversePeriscopeCollider;
    private bool isFacingRight = true;

    void Start()
    {
        reversePeriscopeCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Move();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // Reverse direction
            moveSpeed = -moveSpeed;
            FlipSprite();
        }
    }

    private void Move()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void FlipSprite()
    {
        // Only flip the sprite if the direction changes
        if ((moveSpeed > 0 && !isFacingRight) || (moveSpeed < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
