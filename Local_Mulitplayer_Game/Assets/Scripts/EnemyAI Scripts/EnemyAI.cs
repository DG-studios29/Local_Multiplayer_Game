using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class EnemyAI : MonoBehaviour
{
    public EnemyData enemyData;

    public NavMeshAgent agent;
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
    void Start()
    {

       onEnemySpawn?.Invoke(this.gameObject);

       GetEnemyData();

    }




    // Update is called once per frame
    void Update()
    {
        
    }

    void EnemyDestroy()
    {
        onEnemyDeath?.Invoke(this.gameObject);
        Destroy(gameObject, 0.1f);
    }

    void TakeDamage(float hp)
    {
        health -= hp;

        if(health <= 0)
        {
            EnemyDestroy();
        }
    }


    void GetEnemyData()
    {

        agent = GetComponent<NavMeshAgent>();

        health = enemyData.MaxHealth;
        damage = enemyData.Damage;
        speed = enemyData.MoveSpeed;
        targetPlayerRange = enemyData.TargetPlayerRange;
        openRange = enemyData.OpenRange;
    }

}
