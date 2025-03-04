using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class EnemyAI : MonoBehaviour
{
    public EnemyData enemyData;

    public NavMeshAgent navAgent;
    public GameObject enemyParent; //a ref of something like this to check if friend or foe, will be set when spawned by the player

    public List<GameObject> targetList;
    public List<GameObject> playerTargetList;

    public static event Action<GameObject> onEnemySpawn; //event to invoke enemy spawn
    public static event Action<GameObject> onEnemyDeath;

    public static event Action<EnemyAI> findEnemyTargets; //will add all existing enemy types

    [SerializeField]private float health;
    private float damage;
    private float speed;
    private float targetPlayerRange;
    private float openRange;

    private float attackRange;
    private float attackCooldown = 1f;
    private float time_sinceAttack = 0f;

    [SerializeField] protected GameObject nearestTarget;
    protected float nearestDistance;

    [SerializeField]protected GameObject nearestPlayerTarget;
    protected float nearestPlayerDistance;



    private string playerTag = "Player";

    


    private void OnEnable()
    {
        ItemObject.findEnemies += AddToEnemyList;

        EnemyAI.onEnemySpawn += AddToMyTargetList;
        EnemyAI.onEnemyDeath -= RemoveFromMyTargetList;

        EnemyAI.findEnemyTargets += AddToMyTargetList;
    }

    private void OnDisable()
    {
        ItemObject.findEnemies -= AddToEnemyList;

        EnemyAI.onEnemySpawn -= AddToMyTargetList;
        EnemyAI.onEnemyDeath -= RemoveFromMyTargetList;

        EnemyAI.findEnemyTargets -= AddToMyTargetList;
    }


    void Start()
    {

        navAgent = GetComponent<NavMeshAgent>();
        GetEnemyData();

        navAgent.speed = speed;

        time_sinceAttack = 0f;

        if(enemyParent == null)
        {
            RandomizeParent(); //will randomize its parent
        }

        FindEnemyPlayers(); //A bit of a waste, randomize parent does the same thing ; fix later

        onEnemySpawn?.Invoke(this.gameObject); // alert all listeners that this enemy is spawned

        findEnemyTargets?.Invoke(this); // find all enemy targets that exist elsewhere
        
        

    }




    // Update is called once per frame
    void Update()
    {
        time_sinceAttack += Time.deltaTime;

        //might keep the tracking functions to a global interval or something like that
        NearestTargetTracking();
        NearestPlayerTracking();

        DoTargetChase();

        if(time_sinceAttack > attackCooldown)
        {
            DoAttack();
        }


    }

    void EnemyDestroy()
    {
       
        Destroy(gameObject, 0.1f);
    }

    public void TakeDamage(float hp)
    {
        health -= hp;

        if(health <= 0)
        {
            onEnemyDeath?.Invoke(this.gameObject);
            EnemyDestroy();
        }
    }


    void GetEnemyData()
    {
        health = enemyData.MaxHealth;
        damage = enemyData.Damage;
        speed = enemyData.MoveSpeed;
        targetPlayerRange = enemyData.TargetPlayerRange;
        openRange = enemyData.OpenRange;
        attackRange = enemyData.AttackRange;

        attackCooldown = 1f;
    }


    void FindEnemyPlayers()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(playerTag);

        foreach (GameObject parent in gameObjects)
        {
            if(parent != enemyParent)  //dont add your own parent
            playerTargetList.Add(parent);
        }
    }


    void RandomizeParent()
    {
        List<GameObject> adopters = new List<GameObject>();

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(playerTag);

        foreach(GameObject parent in gameObjects)
        {
            adopters.Add(parent);
        }


        if (adopters.Count > 1)
        {
            int randNo;
            randNo = UnityEngine.Random.Range(0, adopters.Count - 1);
            enemyParent = adopters[randNo];
        }
        else if (adopters.Count == 1)
        {
            enemyParent = adopters[0];
        }
        else
        {
            enemyParent = null;
        }

    }

    //finds all existing enemies, and stores them in the item object's list on its awake
    public void AddToEnemyList(ItemObject itemObject)
    {
        itemObject.AddEnemyToTarget(this.gameObject);
    }



    //Enemies can add other enemies to their list of targets
    public void AddToMyTargetList(GameObject enemy)
    {
        //needs to check if its already in the list, if they are not of the same parent, and if it is not itself


        //make sure a parent exists // wont be a problem when linked to spawning
        if (enemyParent == null) //Note, this is for the object adding to its target list
        {
            RandomizeParent(); //will randomize its parent
        }


        if (this.enemyParent != enemy.GetComponent<EnemyAI>().enemyParent)  // if not siblings, add to enemy and target list
        {
            if (targetList.Count > 0)
            {
                bool alreadyTargeting = false;

                foreach (GameObject target in targetList)
                {
                    if (target == enemy)
                    {
                        alreadyTargeting = true; //no duplicates
                    }
               
                }

                if (!alreadyTargeting) targetList.Add(enemy);  // if we dont find it in out list, we are not already targetting. hence why we add
            }
            else
            {
                targetList.Add(enemy);
                //initialize target tracking
                nearestDistance = Vector3.Distance(transform.position, targetList[0].transform.position); //first element will be used as nearest
                nearestTarget = targetList[0];  // to avoid a case where nearest target does not end up getting set and thus remains null
            }
            
        }
        else
        {
            //Debug.Log("Object parent :  " + parentPlayer + "Enemy :  " + enemy.GetComponent<EnemyAI>().enemyParent);
        }
    }

    public void AddToMyTargetList(EnemyAI enemyAI)
    {
        enemyAI.AddToMyTargetList(this.gameObject);
    }


    //Will remove destoryed enemies from list of targets
    public void RemoveFromMyTargetList(GameObject enemy)
    {
        bool removable = false;
        foreach (GameObject target in targetList)
        {
            if (target == enemy)
            {
                //enemyTargets.Remove(target);  //remove an enemy from the list when its destroyed
                removable = true;
            }
        }

        if (removable) targetList.Remove(enemy);
    }


    void NearestTargetTracking()
    {

        if (targetList.Count == 0)
        {
            nearestTarget = null;
            return;
        }
        else if (targetList.Count == 1)
        {
            nearestDistance = Vector3.Distance(transform.position, targetList[0].transform.position); //first element will be used as nearest
            nearestTarget = targetList[0];  // to avoid a case where nearest target does not end up getting set and thus remains null

        }
        else
        {
            foreach (GameObject target in targetList)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = target;
                }
            }
        }


    }

    void NearestPlayerTracking()
    {
        if (playerTargetList.Count == 0)
        {
            nearestPlayerTarget = null;
            return;
        }
        else if (playerTargetList.Count == 1)
        {
            nearestPlayerDistance = Vector3.Distance(transform.position, playerTargetList[0].transform.position); //first element will be used as nearest
            nearestPlayerTarget = playerTargetList[0];  // to avoid a case where nearest target does not end up getting set and thus remains null

        }
        else
        {
            foreach (GameObject target in playerTargetList)
            {
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < nearestPlayerDistance)
                {
                    nearestPlayerDistance = distance;
                    nearestPlayerTarget = target;
                }
            }
        }
    }

    //needs to make logical decisions in all posible cases of what to target
    void DoTargetChase()
    {
        if (nearestTarget == null && nearestPlayerTarget != null)
        {
            //We have no other enemies to worry about and will just target the player 
            navAgent.SetDestination(nearestPlayerTarget.transform.position);

            /* if (nearestPlayerDistance < targetPlayerRange)
             {
                 navAgent.SetDestination(nearestPlayerTarget.transform.position);
             }*/
        }

        if (nearestTarget != null && nearestPlayerTarget != null)
        {
            //chase the player if its within a certain range no matter what
            if (nearestPlayerDistance < targetPlayerRange)
            {
                navAgent.SetDestination(nearestPlayerTarget.transform.position);
            }

            //else, target the nearest enemy mob
            else if (nearestDistance < nearestPlayerDistance)
            {
                navAgent.SetDestination(nearestTarget.transform.position);
            }
            //or player
            else
            {
                navAgent.SetDestination(nearestPlayerTarget.transform.position);
            }
        }

        //outlier cases
        if (nearestTarget != null && nearestPlayerTarget == null)
        {
            navAgent.SetDestination(nearestTarget.transform.position);
        }

        if(nearestTarget == null && nearestPlayerTarget == null)
        {
            navAgent.SetDestination(this.transform.position);
        }


    }

    //maybe edit distance, target convention here and use etc (distance of nearest target)? nahh
    void DoAttack()
    {

        if(nearestTarget != null && nearestPlayerTarget != null)
        {
            if(nearestDistance < nearestPlayerDistance && nearestDistance < attackRange)
            {
                nearestTarget.GetComponent<EnemyAI>().TakeDamage(damage); //deal damage when in attack range
                time_sinceAttack = 0; //sets the cooldown for this function's condition
            }
            else if(nearestPlayerDistance < nearestDistance && nearestPlayerDistance < attackRange)
            {
                //player take damage
                time_sinceAttack = 0;
            }
        }

        else if (nearestTarget == null && nearestPlayerTarget != null)
        {
            if(nearestPlayerDistance < attackRange)
            {
                //player to take damage
                time_sinceAttack = 0;
            }
        }
        else if(nearestTarget != null && nearestPlayerTarget == null)
        {
            if(nearestDistance < attackRange)
            {
                nearestTarget.GetComponent<EnemyAI>().TakeDamage(damage);
                time_sinceAttack = 0;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,targetPlayerRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position,openRange);


    }

}
