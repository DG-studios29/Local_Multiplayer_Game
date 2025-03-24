using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.OverlapSphere.html
//https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.html
public class Nightshade : HeroBase
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
            // Shadow Dash ability
            Debug.Log("Nightshade dashes through shadows!");
            StartCoroutine(ShadowDash());
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
            // Dark Vortex ability
            Debug.Log("Nightshade summons a Dark Vortex!");
            StartCoroutine(DarkVortex());
            ultimateCooldownTimer = abilities.ultimate.cooldown;

        }
        else
        {
            Debug.LogWarning("Ultimate ability is still on cooldown!");
        }
    }

    private IEnumerator DarkVortex()
    {
        float vortexDuration = 5f;
        float vortexRadius = 10f;
        float pullForce = 10f;

        // Spawn Dark Vortex projectile in front of the player
        Vector3 vortexSpawnPosition = transform.position + transform.forward * 2f; // Adjust distance as needed
        GameObject vortex = Instantiate(abilities.ultimate.projectilePrefab, vortexSpawnPosition, Quaternion.identity);

        // Apply any necessary forces or effects to the vortex
        Rigidbody rb = vortex.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Vortex should stay still
        }

        // Damage and pull effect
        Collider[] hitEnemies = Physics.OverlapSphere(vortexSpawnPosition, vortexRadius);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("Player"))
            {
                if (enemy.gameObject.GetInstanceID() == gameObject.GetInstanceID())
                    continue;

                enemy.GetComponent<PlayerHealth>()?.TakeDamage((int)abilities.ultimate.damage);
                enemy.GetComponent<EnemyAI>()?.TakeDamage((int)abilities.ultimate.damage);

                // Pull enemy toward the vortex
                Rigidbody enemyRb = enemy.GetComponent<Rigidbody>();
                if (enemyRb != null)
                {
                    Vector3 direction = (vortexSpawnPosition - enemy.transform.position).normalized;
                    enemyRb.AddForce(direction * pullForce, ForceMode.VelocityChange);
                }
            }
        }

        yield return new WaitForSeconds(vortexDuration);

        // Destroy vortex after duration
        Destroy(vortex);
    }

    private IEnumerator ShadowDash()
    {
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

        // Spawn Shadow Dash effect behind the player
        Vector3 dashSpawnPosition = originalPosition - transform.forward * 2f; // Adjust distance as needed
        GameObject dashEffect = Instantiate(abilities.ability2.projectilePrefab, dashSpawnPosition, Quaternion.identity);

        // Optionally add force or animation
        Rigidbody rb = dashEffect.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Keep it static
        }

        // Damage effect
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



}