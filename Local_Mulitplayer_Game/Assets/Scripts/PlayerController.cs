using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayerEffect
{
    [Header("Movement Settings")]
    public float moveSpeed = 10f;
    public bool isWalking = true;
    [SerializeField] private LayerMask objectsToCheckAgainst; //for collision detection

    #region Pickup Variables

    bool hasTrail = false;
    Coroutine speedCoroutine;

    #endregion

    //public bool isPunchR = false;

    private Animator animator;
    public Animator Animator => animator;


    private Rigidbody rb;
    private Vector2 movementInput;


    //Gonna store all this stuff in a Player Punches script after merge
    private PlayerPunches playerPunches;



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
            playerPunches.ChargingCall(true);
        }

        if (context.canceled)
        {
            //cancelled on release 
            //cancel charge

            playerPunches.PunchCall();
            //playerPunches.AnimatorChargeClear();

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
            if (!CollidingWithObstacle())
            {
                // Move the player based on the movement input, speed, and fixed time step
                rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            }
            else
            {
                rb.linearVelocity = Vector3.zero;
            }

            // Rotate the player to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f));

            animator.SetBool("isWalking", true);


        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (rb.linearVelocity.y < 0)
        {
            Physics.gravity = new Vector3(0, -45.62f, 0);
        }
        else
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }



    }
    private void Update()
    {
        //// Testing player Health 
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    GetComponent<PlayerHealth>().TakeDamage(10);
        //}

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    GetComponent<PlayerHealth>().Heal(10);
        //}
    }


    /*   private void OnDrawGizmos()
       {
           Vector3 GizmoPos = transform.position + new Vector3(0, 1, 0);

           Gizmos.color = Color.yellow;
           Gizmos.DrawWireSphere(GizmoPos + transform.forward * punchDistance,punchRadius);
       }*/

    private bool CollidingWithObstacle()
    {
        return Physics.Raycast(transform.position + new Vector3(0, .7f, 0), transform.forward, out RaycastHit hitInfo, .5f, objectsToCheckAgainst) ? true : false;
    }

    #region Interface / Pickups

    public void ActivateSpeedBoost(float duration, float speedMultiplier, GameObject trailEffect)
    {
        moveSpeed += speedMultiplier;

        if (!hasTrail)
        {
            trailEffect = Instantiate(trailEffect);
            hasTrail = true;
        }

        trailEffect.transform.SetParent(transform);
        trailEffect.transform.localPosition = new Vector3(0, .01f, 0);
        if (speedCoroutine != null) StopCoroutine(speedCoroutine);
        speedCoroutine = StartCoroutine(SpeedBoostEffect(duration, trailEffect));
    }

    private IEnumerator SpeedBoostEffect(float duration, GameObject trail)
    {
        yield return StartCoroutine(CountHelper(duration));

        moveSpeed = 10f;

        if(trail!=null)
        {
            Destroy(trail);
        }

        hasTrail = false;
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

    public void ActivateShield(float duration, GameObject shield)
    {
        //
    }

    public void GiveHealth(float health)
    {
        //
    }

    public void RefillAbilityBar(float energy)
    {
        //
    }
    #endregion


}