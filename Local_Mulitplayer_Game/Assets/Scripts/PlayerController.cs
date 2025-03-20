using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f; 
    public bool isWalking = true;

  
    public bool isPunchR = false;

    private Animator animator;
    

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

    public void OnPunch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

            Debug.Log("Called Punch");


            //ConfigureClip
            TogglePunch();


            animator.SetTrigger("Punch");

            

            //anim.Anim
        }
       
    }

    void TogglePunch()
    {
        if (!isPunchR)
        {
            animator.SetBool("isPunchR", false);
            isPunchR = true;
        }
        else
        {
            animator.SetBool("isPunchR",true);
            isPunchR = false;
        }
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
    private void Update()
    {
        // Testing player Health 
        if (Input.GetKeyDown(KeyCode.H))
        {
            GetComponent<PlayerHealth>().TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            GetComponent<PlayerHealth>().Heal(10);
        }
    }
}