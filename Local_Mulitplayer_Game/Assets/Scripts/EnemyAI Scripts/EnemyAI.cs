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
    private float attackCooldown;
    private float time_sinceAttack;

    [SerializeField] protected GameObject nearestTarget;
    protected float nearestDistance;



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
        DoTargetChase();
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


    void DoTargetChase()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,targetPlayerRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,openRange);


    }

}
