using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.OverlapSphere.html
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.html
public class Stonewarden : HeroBase
{
  

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
        float duration = 10f;
        float radius = 5f;
        float enemyDetectionRadius = 5f;
        int numberOfRocks = 3;
        float spawnHeight = 10f;
        float fallSpeed = 20f;
        int damage = abilities.ability2.damage;

        if (abilities.ability2.projectilePrefab == null)
        {
            Debug.LogError("Rock Fall projectile prefab is missing.");
            yield break;
        }

        for (int i = 0; i < numberOfRocks; i++)
        {
            // Spawn rock at random position above the area
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-radius, radius), spawnHeight, Random.Range(-radius, radius));
            GameObject rock = Instantiate(abilities.ability2.projectilePrefab, spawnPos, Quaternion.identity);
            Rigidbody rb = rock.GetComponent<Rigidbody>();

            if (rb)
            {
                // Find the nearest enemy excluding the caster
                Collider[] hitEnemies = Physics.OverlapSphere(transform.position, enemyDetectionRadius);
                Transform targetEnemy = null;
                float minDistance = Mathf.Infinity;

                foreach (var enemy in hitEnemies)
                {
                    // Exclude the caster from being a target
                    if (enemy.gameObject.GetInstanceID() == casterID)
                        continue; // Skip the caster

                    // Only consider enemies or players for targeting
                    if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
                    {
                        float distance = Vector3.Distance(spawnPos, enemy.transform.position);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            targetEnemy = enemy.transform;
                        }
                    }
                }

                // Move the rock towards the selected enemy or fall down if no target found
                if (targetEnemy != null)
                {
                    Vector3 direction = (targetEnemy.position - spawnPos).normalized;
                    rb.linearVelocity = direction * fallSpeed;
                }
                else
                {
                    // Fall straight down if no enemy is found
                    rb.linearVelocity = Vector3.down * fallSpeed;
                }
            }

            // Add RockDamage component for collision handling
            Projectile projScript = rock.AddComponent<Projectile>();
            if (projScript) projScript.SetShooter(gameObject);


            projScript.Initialize(gameObject, abilities.ultimate.damage);

            // Ensure the rock is destroyed after a duration
            StartCoroutine(DestroyAfterDuration(rock, duration));
            yield return new WaitForSeconds(0.5f); // Small delay between rock spawns
        }



    }


    private IEnumerator DestroyAfterDuration(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
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
   
            if (enemyID == casterID || hitEnemiesSet.Contains(enemyID))
                continue; // Skip if it's the caster or already hit
           

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
