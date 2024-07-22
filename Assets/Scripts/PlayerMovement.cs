using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float runSpeed = 350;
    
    Vector2 movementInput;
    // cache the player's rigidbody
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Run();
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2(runSpeed * movementInput.x * Time.deltaTime, rb.velocity.y);
        rb.velocity = playerVelocity;
    }

    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
        Debug.Log("Player is moving with input: " + movementInput);
    }
}
