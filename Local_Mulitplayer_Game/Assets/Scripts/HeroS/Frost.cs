using UnityEngine;
using System.Collections;
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.OverlapSphere.html
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.html
public class Frost : HeroBase
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
            Debug.LogWarning("Ice Spikes are still on cooldown!");
        }
    }

    protected override void UseAbility2()
    {
        if (ability2CooldownTimer <= 0f)
        {
            Debug.Log("?? Frost launches Ice Spikes!");
            StartCoroutine(IceSpikes());
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
            Debug.Log("???? Frost unleashes Ice Age!");
            StartCoroutine(IceAge());
            ultimateCooldownTimer = abilities.ultimate.cooldown;
            
        }
        else
        {
            Debug.LogWarning("Ultimate ability is still on cooldown!");
        }
    }

    private IEnumerator IceSpikes()
    {
        if (abilities.ability2.projectilePrefab == null)
        {
            Debug.LogError("Ice Spikes prefab is missing! Assign it in the HeroAbility scriptable object.");
            yield break;
        }
        Vector3 spawnPosition = transform.position + transform.forward * 2; // Spawn in front of the player, 2 units away
        GameObject spike = Instantiate(abilities.ability2.projectilePrefab, spawnPosition, Quaternion.identity);
        Projectile projScript = spike.GetComponent<Projectile>();

        if (projScript != null)
        {
            projScript.Initialize(gameObject, abilities.ability2.damage);
        }


        casterID = gameObject.GetInstanceID();
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 3f);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.gameObject.GetInstanceID() != casterID)
            {
                enemy.GetComponent<PlayerHealth>()?.TakeDamage(abilities.ability2.damage);
                enemy.GetComponent<EnemyAI>()?.TakeDamage(abilities.ability2.damage);
            }
        }

        yield return new WaitForSeconds(1f);
    }

    private IEnumerator IceAge()
    {
        float freezeDuration = 10f;
        float radius = 6f;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject)
            {
                Debug.Log($"?? Freezing {enemy.name}");


                enemy.GetComponent<PlayerHealth>()?.TakeDamage(abilities.ultimate.damage);
                enemy.GetComponent<EnemyAI>()?.TakeDamage(abilities.ultimate.damage);

                // Apply freeze effect 
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.FreezeAll;
                    StartCoroutine(UnfreezeAfterDelay(rb, freezeDuration));
                }
            }
        }

        yield return null;
    }

    private IEnumerator UnfreezeAfterDelay(Rigidbody rb, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;

        }
    }
}
