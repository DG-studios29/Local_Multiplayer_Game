using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField]protected ItemData itemData; //item data to be used

    protected float damage;
    protected float radius;
    private float s_timerSinceAttack = 0f;
    protected float s_attackRate;

    private Transform playerOrigin; //starting postion
    public List<GameObject> enemyTargets;


    protected float nearestTarget;
  
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
        InitializeObject();
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


    void DoAttack()
    {
        if (s_timerSinceAttack >= s_attackRate)
        {
            //if enough time has elapsed

            //if there is an enemy within range
            //if()

        }



    }

    void FindNearest()
    {
        if(enemyTargets.Count == 0)
        {
            return;
        }

        foreach(GameObject target in enemyTargets)
        {
            var distance = Vector3.Distance(transform.position,target.transform.position);
            if(distance < nearestTarget)
            {

            }
        }
    }
   
   
}
