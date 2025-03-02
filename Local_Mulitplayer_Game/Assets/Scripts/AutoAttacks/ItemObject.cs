using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemObject : MonoBehaviour
{
    [SerializeField]protected ItemData itemData; //item data to be used

    protected float damage;
    protected float radius;
    private float s_timerSinceAttack = 0f;
    protected float s_attackRate;

    private Transform playerOrigin; //starting postion
    public List<GameObject> enemyTargets;


    protected GameObject nearestTarget;
    protected float nearestDistance;
    private float nearestTemp;

    public static event Action<ItemObject> findEnemies;

  
    private void OnEnable()
    {
        EnemyAI.onEnemySpawn += AddEnemyToTarget;
    }

    private void OnDisable()
    {
        EnemyAI.onEnemySpawn -= AddEnemyToTarget;
    }


    void Start()
    {
        s_timerSinceAttack = 0f;
        InitializeObject();

        findEnemies?.Invoke(this);
        Debug.Log("Called from Item Object");

    }

    // Update is called once per frame
    void Update()
    {
        s_timerSinceAttack += Time.deltaTime;

        DoAttack();
    }

    public void InitializeObject()
    {
        //kinda our constructor

        damage = itemData.atkDamage;
        radius = itemData.atkRadius;
        s_attackRate = itemData.atkRate;

        enemyTargets = new List<GameObject>();
    }

    public void AddEnemyToTarget(GameObject enemy)
    {

        enemyTargets.Add(enemy);
    }

    public void RemoveEnemyFromTargets(GameObject enemy)
    {
        foreach(GameObject target in enemyTargets)
        {
            if(target == enemy)
            {
                enemyTargets.Remove(target);  //remove an enemy from the list when its destroyed
            }
        }
    }


    public virtual void DoAttack()
    {
        if (s_timerSinceAttack >= s_attackRate)
        {
            //if enough time has elapsed
            FindNearest();

            //if the nearest enemy is within range
            if(nearestTarget != null)
            {
                if(Vector3.Distance(gameObject.transform.position, nearestTarget.transform.position) <= radius)
                {
                    // Visually Show something

                    //Take Damage
                    nearestTarget.GetComponent<EnemyAI>().TakeDamage(damage);
                    Debug.Log("Taking damage from: \t" + nearestTarget.name);

                    s_timerSinceAttack = 0; //reset the timer
                }
            }

        }



    }

    void FindNearest()
    {
        if(enemyTargets.Count == 0)
        {
            nearestTarget = null;
            return;
        }
        else
        {
            nearestDistance = Vector3.Distance(transform.position, enemyTargets[0].transform.position); //first element will be used as nearest
        }

        foreach(GameObject target in enemyTargets)
        {
            var distance = Vector3.Distance(transform.position,target.transform.position);
            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestTarget = target;
            }
        }
    }
   
   
}
