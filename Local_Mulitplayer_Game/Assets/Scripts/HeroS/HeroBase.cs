using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class HeroBase : MonoBehaviour
{
    public HeroAbility abilities;
    private PlayerInput playerInput;
    private Transform projectileSpawnPoint;
    private float projectileSpeed = 10f;

    public float ability1CooldownTimer = 0f;
    public float ability2CooldownTimer = 0f;
    public float ultimateCooldownTimer = 0f;

    protected virtual void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Ability1"].performed += ctx => UseAbility1();
        playerInput.actions["Ability2"].performed += ctx => UseAbility2();
        playerInput.actions["Ultimate"].performed += ctx => UseUltimate();

        projectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
        if (!projectileSpawnPoint)
        {
            Debug.LogError($"{gameObject.name} is missing a ProjectileSpawnPoint!");
        }
    }

    protected abstract void UseAbility1();
    protected abstract void UseAbility2();
    protected abstract void UseUltimate();

    public void ShootProjectile(Ability ability)
    {
        if (ability.projectilePrefab == null)
        {
            Debug.LogError($"{ability.abilityName} projectile prefab is missing! Assign it in the HeroAbility scriptable object.");
            return;
        }

        // Instantiate the projectile and initialize it with the correct damage
        GameObject projectile = Instantiate(ability.projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Get the Rigidbody component to apply velocity
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Set the velocity to move the projectile forward (use a direction, e.g., forward)
            rb.linearVelocity = transform.forward * projectileSpeed; // Adjust the speed (10f) as necessary
        }

        // Initialize the projectile with the shooter and damage
        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Initialize(gameObject, ability.damage); // Pass the shooter and damage value
        }
    }


    public IEnumerator CooldownCoroutine(int abilityIndex)
    {
        float cooldownTime = 0f;

        // Set the cooldown time based on the ability index
        if (abilityIndex == 1) cooldownTime = abilities.ability1.cooldown;
        if (abilityIndex == 2) cooldownTime = abilities.ability2.cooldown;
        if (abilityIndex == 3) cooldownTime = abilities.ultimate.cooldown;

        // Wait for the cooldown to finish
        while (cooldownTime > 0f)
        {
            cooldownTime -= Time.deltaTime;
            yield return null;
        }

        // Reset the corresponding cooldown timer to 0 after it's done
        if (abilityIndex == 1) ability1CooldownTimer = 0f;
        if (abilityIndex == 2) ability2CooldownTimer = 0f;
        if (abilityIndex == 3) ultimateCooldownTimer = 0f;

        Debug.Log($"Ability {abilityIndex} is ready to use!");
    }

}
