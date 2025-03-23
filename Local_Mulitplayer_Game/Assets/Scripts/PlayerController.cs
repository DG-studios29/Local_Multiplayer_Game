<<<<<<< Wandile-Branch-2
using System.Collections;
=======
using System;
>>>>>>> main
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IPlayerEffect
{
    [Header("Movement Settings")]


    #region Pickup Variables

    private float dur = 0;
    bool hasTrail = false;

    #endregion 

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
        Debug.Log("Called Punch");
        animator.SetTrigger("Punch");
        GetComponent<PlayerHealth>().TakeDamage(10);
    }

   

    // Make the player move
    private void FixedUpdate()
    {
        // Convert the 2D movement input to a 3D movement direction
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        isWalking = moveDirection.magnitude > 0.1f;
        if (isWalking)
        {
            if(!CollidingWithObstacle())
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


    }
    private void Update()
    {
        //// Testing player Health 
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    GetComponent<PlayerHealth>().TakeDamage(10);
    }

<<<<<<< Wandile-Branch-2
    private bool CollidingWithObstacle()
    {
        return Physics.Raycast(transform.position + new Vector3(0,.7f, 0), transform.forward, out RaycastHit hitInfo, .5f, objectsToCheckAgainst)? true: false;
    }

    #region Interface / Pickups

    public void ActivateSpeedBoost(float duration, float speedMultiplier, GameObject trailEffect)
    {
        moveSpeed += speedMultiplier;
        dur += duration;
        
        if(!hasTrail)
        {
            GameObject trail = Instantiate(trailEffect);

            trailEffect = null;
            hasTrail = true;

            trail.transform.SetParent(transform);
            trail.transform.localPosition = new Vector3(0, .01f, 0);

            StartCoroutine(SpeedBoostEffect(dur, trail));
        }
    }

    private IEnumerator SpeedBoostEffect(float duration, GameObject trail)
    {
        yield return StartCoroutine(CountHelper(duration));

        moveSpeed = 10f;
        dur = 0;
        Destroy(trail);
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

    public void ActivateShield(float duration)
    {
        throw new System.NotImplementedException();
    }

    public void GiveHealth(float health)
    {
        throw new System.NotImplementedException();
    }

    public void RefillAbilityBar(float energy)
    {
        throw new System.NotImplementedException();
    }
    #endregion
=======
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    GetComponent<PlayerHealth>().Heal(10);
        //}
    }
>>>>>>> main
}