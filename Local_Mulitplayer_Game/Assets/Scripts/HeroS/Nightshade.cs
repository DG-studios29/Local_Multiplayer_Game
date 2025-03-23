using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Nightshade : HeroBase
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
            // Shadow Dash ability
            Debug.Log("Nightshade dashes through shadows!");
            StartCoroutine(ShadowDash());
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
            // Dark Vortex ability
            Debug.Log("Nightshade summons a Dark Vortex!");
            StartCoroutine(DarkVortex());
            ultimateCooldownTimer = abilities.ultimate.cooldown; 
            StartCoroutine(CooldownCoroutine(3));
        }
        else
        {
            Debug.LogWarning("Ultimate ability is still on cooldown!");
        }
    }

    private IEnumerator ShadowDash()
    {
        // Implement the dash logic here, e.g., move Nightshade quickly across the screen
        float dashSpeed = 10f;
        float dashDuration = 1f;

        Vector3 originalPosition = transform.position;
        Vector3 dashTarget = originalPosition + transform.forward * dashSpeed;

        float timer = 0f;
        while (timer < dashDuration)
        {
            transform.position = Vector3.Lerp(originalPosition, dashTarget, timer / dashDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        transform.position = dashTarget;

        // Now leave a damage radius at the start of the dash
        StartCoroutine(DamageRadius(originalPosition));
        Debug.Log("Nightshade completes the Shadow Dash!");
    }

    private IEnumerator DamageRadius(Vector3 position)
    {
        float radius = 3f;
        float duration = 2f;

        Collider[] hitEnemies = Physics.OverlapSphere(position, radius);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
            {
              
                if (enemy.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    continue;

                
                enemy.GetComponent<PlayerHealth>()?.TakeDamage((int)abilities.ability2.damage);
                enemy.GetComponent<EnemyAI>()?.TakeDamage((int)abilities.ability2.damage);
            }
        }

       
        yield return new WaitForSeconds(duration);
    }

    private IEnumerator DarkVortex()
    {
        
        float vortexDuration = 5f;
        float vortexRadius = 10f;
        float pullForce = 10f;

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, vortexRadius);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
            {
                
                if (enemy.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    continue;

               
                enemy.GetComponent<PlayerHealth>()?.TakeDamage((int)abilities.ultimate.damage);
                enemy.GetComponent<EnemyAI>()?.TakeDamage((int)abilities.ultimate.damage);

                // Pull enemy 
                Rigidbody rb = enemy.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 direction = (transform.position - enemy.transform.position).normalized;
                    rb.AddForce(direction * pullForce, ForceMode.VelocityChange);
                }
            }
        }

        
        yield return new WaitForSeconds(vortexDuration);
    }
}
