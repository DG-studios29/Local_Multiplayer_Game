using UnityEngine;

public class PlayerPunches : MonoBehaviour
{

    private PlayerController playerController;
    private Animator animator;


    //Punching
    public bool isPunchR = false;
    [SerializeField] private float punchDamage;

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




    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        lastPunchTimer += Time.deltaTime;
    }


    public void PunchCall()
    {
        if (lastPunchTimer < punchCooldown)
        {
            //we wont punch if cooldown has not passed
            return;
        }

        lastPunchTimer = 0;

        Debug.Log("Called Punch");


        //ConfigureClip
        TogglePunch();


        animator.SetTrigger("Punch");

        punchPosition = transform.position + new Vector3(0, 1, 0);

        if (Physics.SphereCast(punchPosition, punchRadius, transform.forward, out hit, 3f, playerMask))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                GameObject target = hit.collider.gameObject;
                PlayerHealth targetHealth = target.GetComponent<PlayerHealth>();

                targetHealth.TakeDamage((int)punchDamage); 

                PlayerController targetControl = target.GetComponent<PlayerController>();
                targetControl.Animator.SetTrigger("Hit");


                punchForce = distancePushed / timePushed;
                Vector3 velocity = punchForce * hit.rigidbody.mass * transform.forward;


                hit.rigidbody.AddForce(velocity, ForceMode.Impulse);
            }

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
            animator.SetBool("isPunchR", true);
            isPunchR = false;
        }
    }



    private void OnDrawGizmos()
    {
        Vector3 GizmoPos = transform.position + new Vector3(0, 1, 0);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(GizmoPos + transform.forward * punchDistance, punchRadius);
    }

}
