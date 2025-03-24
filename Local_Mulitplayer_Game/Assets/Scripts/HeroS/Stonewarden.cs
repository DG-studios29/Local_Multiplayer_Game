using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.OverlapSphere.html
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.html
public class Stonewarden : HeroBase
{
    private int casterID;

    protected override void UseAbility1()
    {
        if (ability1CooldownTimer <= 0f)
        {
            ShootProjectile(abilities.ability1);
            ability1CooldownTimer = abilities.ability1.cooldown;
           
        }
        else
        {
            Debug.LogWarning("Ability 1 is still on cooldown!");
        }
    }

    protected override void UseAbility2()
    {
        if (ability2CooldownTimer <= 0f)
        {
            // Rock Fall ability (rocks fall from the sky and hit enemies)
            Debug.Log("Stonewarden calls down a Rock Fall!");
            StartCoroutine(RockFall());
            ability2CooldownTimer = abilities.ability2.cooldown; 
            
        }
        else
        {
            Debug.LogWarning("Ability 2 is still on cooldown!");
        }
    }

    protected override void UseUltimate()
    {
        if (ultimateCooldownTimer <= 0f)
        {
            // Earthquake ability with pushback effect
            Debug.Log("Stonewarden causes an Earthquake!");
            StartCoroutine(Earthquake());
            ultimateCooldownTimer = abilities.ultimate.cooldown;
            
        }
        else
        {
            Debug.LogWarning("Ultimate ability is still on cooldown!");
        }
    }

    private IEnumerator RockFall()
    {
        float fallDuration = 1f;
        float rockRadius = 5f;
        float damage = abilities.ability2.damage;

        if (abilities.ability2.projectilePrefab == null)
        {
            Debug.LogError("Rock Fall projectile prefab is missing.");
            yield break;
        }

        Collider[] enemies = Physics.OverlapSphere(transform.position, rockRadius);
        GameObject targetEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider enemy in enemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetEnemy = enemy.gameObject;
                }
            }
        }

        if (targetEnemy == null) yield break;

        // Spawn the rock **above** the target enemy
        Vector3 fallPosition = targetEnemy.transform.position + Vector3.up * 10f; // 10 units above
        GameObject rock = Instantiate(abilities.ability2.projectilePrefab, fallPosition, Quaternion.identity);

        if (rock == null) yield break;

        Rigidbody rockRb = rock.GetComponent<Rigidbody>();
        if (rockRb != null)
        {
            rockRb.linearVelocity = Vector3.down * 20f; // Make it fall quickly
        }

        float timer = 0f;
        while (timer < fallDuration)
        {
            if (rock == null) break;  // Exit if the rock is destroyed
            timer += Time.deltaTime;
            yield return null;
        }

        if (rock == null) yield break;

        Collider[] hitEnemies = Physics.OverlapSphere(rock.transform.position, rockRadius);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
            {
                if (enemy.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    continue;

                enemy.GetComponent<PlayerHealth>()?.TakeDamage((int)damage);
                enemy.GetComponent<EnemyAI>()?.TakeDamage((int)damage);
            }
        }

        Destroy(rock);
        Debug.Log("Rock Fall hit the enemy!");
    }



    private IEnumerator Earthquake()
    {
        float quakeDuration = 3f;
        float quakeRadius = 7f;
        float pushForce = 10f;

        // Start in the direction the player is facing
        Vector3 quakeDirection = transform.forward;
        Vector3 quakeStartPosition = transform.position + quakeDirection * 3f;

        // Create the visual effect if you have one
        if (abilities.ultimate.projectilePrefab != null)
        {
            GameObject quakeEffect = Instantiate(abilities.ultimate.projectilePrefab, quakeStartPosition, Quaternion.identity);
            Destroy(quakeEffect, quakeDuration);
        }

        // Track hit enemies to avoid multiple damage applications
        HashSet<int> hitEnemiesSet = new HashSet<int>();

        // Apply damage and pushback **only once per enemy**
        Collider[] hitEnemies = Physics.OverlapSphere(quakeStartPosition, quakeRadius);
        foreach (Collider enemy in hitEnemies)
        {
            int enemyID = enemy.gameObject.GetInstanceID();
            if (enemyID == gameObject.GetInstanceID() || hitEnemiesSet.Contains(enemyID))
                continue; // Skip self and already hit enemies

            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
            {
                hitEnemiesSet.Add(enemyID); // Mark as hit

                // Apply damage
                enemy.GetComponent<PlayerHealth>()?.TakeDamage((int)abilities.ultimate.damage);
                enemy.GetComponent<EnemyAI>()?.TakeDamage((int)abilities.ultimate.damage);

                // Apply pushback effect
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = (enemy.transform.position - quakeStartPosition).normalized;
                    rb.AddForce(direction * pushForce, ForceMode.Impulse);
                }
            }
        }

        yield return new WaitForSeconds(quakeDuration);
    }


}
