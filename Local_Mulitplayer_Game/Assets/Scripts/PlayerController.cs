using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayerEffect
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public bool isWalking = true;
    private Animator animator;
    private float dur = 0;
    private Rigidbody rb;
    private Vector2 movementInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {

        movementInput = context.ReadValue<Vector2>();

    }

    // Make the player move
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


    public void ActivateSpeedBoost(float duration, float speedMultiplier)
    {
        moveSpeed += speedMultiplier;
        dur += duration;

        StartCoroutine(SpeedBoostEffect(dur));
    }

    private IEnumerator SpeedBoostEffect(float duration)
    {
        yield return StartCoroutine(CountHelper(duration));

        moveSpeed = 10f;
        dur = 0;
    }

    private IEnumerator CountHelper(float dur)
    {
        float t = 0;
        while (t < dur)
        {
            t += Time.deltaTime;
            yield return null;
        }
    }
}