using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.OverlapSphere.html
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.html
public class Blazeheart : HeroBase
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
            Debug.Log("Blazeheart uses Fire Burst!");
            StartCoroutine(FireBurst());
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
            Debug.Log("Blazeheart unleashes Firestorm!");
            StartCoroutine(Firestorm());
            ultimateCooldownTimer = abilities.ultimate.cooldown; 
            
        }
        else
        {
            Debug.LogWarning("Ultimate ability is still on cooldown!");
        }
    }


    private IEnumerator FireBurst()
    {
        float duration = 5f;
        float radius = 5f;
        float pushForce = 5f;

        if (abilities.ability2.projectilePrefab == null)
        {
            Debug.LogError("Fire Burst prefab is missing!");
            yield break;
        }

        // Spawn FireBurst in front of the player
        Vector3 spawnPosition = transform.position + transform.forward * 2f;
        GameObject fireBurst = Instantiate(abilities.ability2.projectilePrefab, spawnPosition, Quaternion.identity);
        fireBurst.transform.localScale = Vector3.zero;

        casterID = gameObject.GetInstanceID();
        HashSet<int> hitEnemiesSet = new HashSet<int>(); // Track hit enemies

        float timer = 0f;
        while (timer < duration)
        {
            if (fireBurst == null) yield break;

            fireBurst.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * radius, timer / duration);

            Collider[] hitEnemies = Physics.OverlapSphere(spawnPosition, radius);
            foreach (Collider enemy in hitEnemies)
            {
                int enemyID = enemy.gameObject.GetInstanceID();
                if (enemyID == casterID || hitEnemiesSet.Contains(enemyID))
                    continue; // Skip if it's the caster or already hit

                if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
                {
                    hitEnemiesSet.Add(enemyID); // Mark as hit

                    // Apply damage
                    enemy.GetComponent<PlayerHealth>()?.TakeDamage((int)abilities.ability2.damage);
                    enemy.GetComponent<EnemyAI>()?.TakeDamage((int)abilities.ability2.damage);

                    // Apply knockback
                    Rigidbody rb = enemy.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        Vector3 knockbackDirection = (enemy.transform.position - spawnPosition).normalized;
                        rb.AddForce(knockbackDirection * pushForce, ForceMode.Impulse);
                    }
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        Destroy(fireBurst);
    }




    private IEnumerator Firestorm()
    {
        float duration = 10f;
        float radius = 10f;
        float enemyDetectionRadius = 10f;

        for (int i = 0; i < 5; i++)
        {
            // Spawn position of the fireball within the radius
            Vector3 spawnPos = transform.position + new Vector3(Random.Range(-radius, radius), 5, Random.Range(-radius, radius));
            GameObject fireball = Instantiate(abilities.ultimate.projectilePrefab, spawnPos, Quaternion.identity);
            Rigidbody rb = fireball.GetComponent<Rigidbody>();

            if (rb)
            {
                // Find enemies in the area 
                Collider[] hitEnemies = Physics.OverlapSphere(transform.position, enemyDetectionRadius);
                Transform targetEnemy = null;

                foreach (var enemy in hitEnemies)
                {
                    if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
                    {
                        targetEnemy = enemy.transform;
                        break; 
                    }
                }

                if (targetEnemy != null)
                {
                    // Move the fireball towards the enemy
                    Vector3 direction = (targetEnemy.position - spawnPos).normalized;
                    rb.linearVelocity = direction * 10f; 
                }
                else
                {
                    // Move the fireball towards the edge of the radius
                    Vector3 direction = (spawnPos - transform.position).normalized;
                    rb.linearVelocity = direction * 10f; 
                }
            }

           
            Projectile projScript = fireball.GetComponent<Projectile>();
            if (projScript) projScript.SetShooter(gameObject);

            
            projScript.Initialize(gameObject, abilities.ultimate.damage);

            StartCoroutine(DestroyAfterDuration(fireball, duration));
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator DestroyAfterDuration(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }

  
}
