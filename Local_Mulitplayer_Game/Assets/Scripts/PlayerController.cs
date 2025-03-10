using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f; // Movement speed for the player
    public bool isWalking = true;
    private Animator animator;

    private Rigidbody rb; // Reference to the Rigidbody component
    private Vector2 movementInput; // Stores player movement input from Input System



    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); // Gets the Rigidbody component attached to the GameObject
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        movementInput = context.ReadValue<Vector2>(); // Reads and stores movement input from the player

    }

    private void FixedUpdate()
    {
        // Convert the 2D movement input to a 3D movement direction
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        isWalking = moveDirection.magnitude > 0.1f;
        if (isWalking)
        {
            // Move the player based on the movement input, speed, and fixed time step
            rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

            // Rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f));

            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }


    }

}