using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Movement speed for the player

    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector2 movementInput; // Stores player movement input from Input System

    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Gets the Rigidbody component attached to the GameObject
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>(); // Reads and stores movement input from the player
    }

    private void FixedUpdate()
    {
        // Convert the 2D movement input to a 3D movement direction
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);

        // Move the player based on the movement input, speed, and fixed time step
        rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

}