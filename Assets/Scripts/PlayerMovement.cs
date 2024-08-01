using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 350f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float climbSpeed = 200f;
    [SerializeField] private Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;

    private GameManager gameSession;

    private Animator animator;
    private CapsuleCollider2D bodyCollider;
    private BoxCollider2D feetCollider;
    private Rigidbody2D rb;
    private Vector2 movementInput;
    private bool isAlive = true;
    private float initialGravityScale;
    private bool canDoubleJump = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gameSession = FindObjectOfType<GameManager>();
        initialGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        if (!isAlive) return;
        Run();
        FlipSprite();
        Climb();
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(runSpeed * movementInput.x * Time.deltaTime, rb.velocity.y);
        rb.velocity = playerVelocity;
        animator.SetBool("isRunning", Mathf.Abs(movementInput.x) > Mathf.Epsilon);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    private void Climb()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            animator.SetBool("isClimbing", false);
            rb.gravityScale = initialGravityScale;
            return;
        }

        float verticalInput = movementInput.y;
        Vector2 climbVelocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed * Time.deltaTime);
        rb.velocity = climbVelocity;
        rb.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    private void OnMove(InputValue value)
    {
        if (!isAlive) return;
        movementInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (!isAlive) return;
        if (value.isPressed)
        {
            if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                rb.velocity += Vector2.up * jumpForce;
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.velocity += Vector2.up * jumpForce;
                canDoubleJump = false;
            }
        }
    }

    private void OnFire(InputValue value)
    {
        if (!isAlive) return;

        // Determine the direction based on player facing direction
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // Instantiate and initialize the projectile
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Bullet>().Initialize(direction, projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies") || collision.gameObject.layer == LayerMask.NameToLayer("Hazards"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        animator.SetTrigger("Die");
        rb.velocity = deathKick;
        GameObject.Find("CameraShake").GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        
        gameSession.ProcessPlayerDeath();
    }
}
