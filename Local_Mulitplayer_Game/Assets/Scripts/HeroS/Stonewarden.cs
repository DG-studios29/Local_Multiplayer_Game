using System.Collections;
using UnityEngine;

public class Stonewarden : HeroBase
{
    private int casterID;

    protected override void UseAbility1()
    {
        if (ability1CooldownTimer <= 0f)
        {
            ShootProjectile(abilities.ability1);
            ability1CooldownTimer = abilities.ability1.cooldown; 
            StartCoroutine(CooldownCoroutine(1));
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
            StartCoroutine(CooldownCoroutine(2));
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
            StartCoroutine(CooldownCoroutine(3));
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

        // Find the nearest enemy to target
        Collider[] enemies = Physics.OverlapSphere(transform.position, rockRadius);
        GameObject targetEnemy = null;
        float minDistance = Mathf.Infinity;

        // Find the closest enemy within the rock radius
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

        // Calculate the direction from the caster to the enemy
        Vector3 fallDirection = (targetEnemy.transform.position - transform.position).normalized;

       
        Vector3 fallPosition = transform.position + new Vector3(Random.Range(-rockRadius, rockRadius), 10f, Random.Range(-rockRadius, rockRadius));

      
        GameObject rockPrefab = abilities.ability2.projectilePrefab;
        GameObject rock = Instantiate(rockPrefab, fallPosition, Quaternion.identity);

        // rock falling towards the target enemy
        float timer = 0f;
        while (timer < fallDuration)
        {
            rock.transform.position = Vector3.Lerp(fallPosition, targetEnemy.transform.position, timer / fallDuration);
            timer += Time.deltaTime;
            yield return null;
        }

       
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

        // Starts in the direction the player is facing
        Vector3 quakeDirection = transform.forward;  
        Vector3 quakeStartPosition = transform.position + quakeDirection * 3f;  

        Collider[] hitEnemies = Physics.OverlapSphere(quakeStartPosition, quakeRadius);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
            {
                
                if (enemy.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    continue;

                
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
