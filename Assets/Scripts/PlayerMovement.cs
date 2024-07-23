using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 350;
    private Animator animator;
    
    Vector2 movementInput;
    // cache the player's rigidbody
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Run();
        FlipSprite();
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
}
