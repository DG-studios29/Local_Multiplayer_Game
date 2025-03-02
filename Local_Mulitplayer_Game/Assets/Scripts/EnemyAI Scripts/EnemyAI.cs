using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class EnemyAI : MonoBehaviour
{
    public EnemyData enemyData;

    public NavMeshAgent navAgent;
    public GameObject parentPlayer; //a ref of something like this to check if friend or foe

    public List<GameObject> targetList;

    public static event Action<GameObject> onEnemySpawn; //event to invoke enemy spawn
    public static event Action<GameObject> onEnemyDeath;

    private float health;
    private float damage;
    private float speed;
    private float targetPlayerRange;
    private float openRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void OnEnable()
    {
        ItemObject.findEnemies += AddToEnemyList;
    }

    private void OnDisable()
    {
        ItemObject.findEnemies -= AddToEnemyList;
    }


    void Start()
    {

        navAgent = GetComponent<NavMeshAgent>();
        GetEnemyData();

        navAgent.speed = speed;
        
        onEnemySpawn?.Invoke(this.gameObject);


    }




    // Update is called once per frame
    void Update()
    {
        DoTargetChase();
    }

    void EnemyDestroy()
    {
       
        Destroy(gameObject, 0.5f);
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
    }

    //finds all existing enemies
    public void AddToEnemyList(ItemObject itemObject)
    {
        itemObject.AddEnemyToTarget(this.gameObject);
    }

    void DoTargetChase()
    {

    }

}
