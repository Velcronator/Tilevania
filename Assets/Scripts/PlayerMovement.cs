using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 350f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float climbSpeed = 200f;

    private Animator animator;
    private new CapsuleCollider2D collider2D;
    private bool canDoubleJump = true;


    Vector2 movementInput;
    private Rigidbody2D rb;
    private float initialGravityScale; // Variable to store the initial gravity scale

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<CapsuleCollider2D>();

        initialGravityScale = rb.gravityScale; // Store the initial gravity scale
    }

    private void Update()
    {
        Run();
        FlipSprite();
        Climb();
    }

    private void Climb()
    {
        // if the player is not touching the ladder, do nothing
        if (!collider2D.IsTouchingLayers(LayerMask.GetMask("Climb")))
        {
            animator.SetBool("isClimbing", false);
            rb.gravityScale = initialGravityScale; // Set the gravity scale to the initial value
            return;
        }

        float verticalInput = movementInput.y;
        Vector2 climbVelocity = new Vector2(rb.velocity.x, verticalInput * climbSpeed * Time.deltaTime);
        rb.velocity = climbVelocity;
        rb.gravityScale = 0;

        bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(runSpeed * movementInput.x * Time.deltaTime, rb.velocity.y);
        rb.velocity = playerVelocity;
        animator.SetBool("isRunning", movementInput.x != 0);
    }

    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            // Check if the player is on the ground using IsTouchingLayer
            if (collider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                // Player is on the ground, perform the jump
                rb.velocity += Vector2.up * jumpForce;
                canDoubleJump = true; // Reset double jump
            }
            else if (canDoubleJump)
            {
                // Player is not on the ground, perform the double jump
                rb.velocity = new Vector2(rb.velocity.x, 0); // Reset vertical velocity
                rb.velocity += Vector2.up * jumpForce;
                canDoubleJump = false; // Disable double jump
            }
        }
    }
}
