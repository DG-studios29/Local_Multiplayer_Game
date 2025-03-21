using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public bool isWalking = true;


    //public bool isPunchR = false;

    private Animator animator;
    public Animator Animator => animator;


    private Rigidbody rb;
    private Vector2 movementInput;


    //Gonna store all this stuff in a Player Punches script after merge
    private PlayerPunches playerPunches;
/*
    //Punching
    [SerializeField] private float punchRadius;
    [SerializeField] private float punchDistance;
    [SerializeField] private LayerMask playerMask;
    Vector3 punchPosition;
    private RaycastHit hit;

    //Punch Force
    private float punchForce;
    [SerializeField] private float timePushed;
    [SerializeField] private float distancePushed;

    //Punch Control - Anti-Spam cooldown
    private float punchCooldown = 0.25f; //animation time
    private float lastPunchTimer = 0.25f; 

    //Punch Control - Critical-Hit Holding
*/

   


    private void Awake()
    {
        rb = GetComponent<Rigidbody>(); 
     

        animator = GetComponent<Animator>();

        playerPunches = GetComponent<PlayerPunches>();

    }

    public void OnMove(InputAction.CallbackContext context)
    {

        movementInput = context.ReadValue<Vector2>(); 

    }

    public void OnPunch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {


            playerPunches.PunchCall();



         /*   if(lastPunchTimer < punchCooldown)
            {
                //we wont punch if cooldown has not passed
                return;
            }

            lastPunchTimer = 0;

            Debug.Log("Called Punch");


            //ConfigureClip
            TogglePunch();
            

            animator.SetTrigger("Punch");

            punchPosition = transform.position + new Vector3(0,1,0);

            if(Physics.SphereCast(punchPosition, punchRadius, transform.forward,out hit, 3f, playerMask))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    var target = hit.collider.gameObject;
                    var targetHealth = target.GetComponent<PlayerHealth>();
                    targetHealth.TakeDamage(10); //set it to what we need
                    
                    PlayerController targetControl = target.GetComponent<PlayerController>();
                    targetControl.animator.SetTrigger("Hit");


                    punchForce = distancePushed / timePushed;
                    Vector3 velocity = punchForce * hit.rigidbody.mass * transform.forward;

                    
                    hit.rigidbody.AddForce(velocity,ForceMode.Impulse);
                }

            }
            */

            //anim.Anim
        }
       
    }

/*    void TogglePunch()
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
*/



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
        //lastPunchTimer += Time.deltaTime;

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


 /*   private void OnDrawGizmos()
    {
        Vector3 GizmoPos = transform.position + new Vector3(0, 1, 0);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GizmoPos + transform.forward * punchDistance,punchRadius);
    }*/
}