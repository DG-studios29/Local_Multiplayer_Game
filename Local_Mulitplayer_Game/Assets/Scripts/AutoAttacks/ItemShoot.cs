using UnityEngine;

public class ItemShoot : ItemObject
{


    protected GameObject trailRend;
    protected GameObject projectileObject;
    protected GameObject particleFX;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        Debug.Log("called");

        trailRend = itemData.trailRenderer;
        projectileObject = itemData.projectilePrefab;
        particleFX = itemData.particleFX;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void DoAttack()
    {
        if (s_timerSinceAttack > s_attackRate)
        {
            //if enough time has elapsed
            FindNearest();

            //if the nearest enemy is within range
            if (nearestTarget != null)
            {
                if (Vector3.Distance(gameObject.transform.position, nearestTarget.transform.position) <= radius)
                {
                    // Visually Show something

                    //Take Damage
                    nearestTarget.GetComponent<EnemyAI>().TakeDamage(damage);
                    Debug.Log("Dealing damage to: \t" + nearestTarget.name);

                    s_timerSinceAttack = 0; //reset the timer
                    nearestTarget = null; //nulling the nearest target
                }
            }

        }

    }
}
