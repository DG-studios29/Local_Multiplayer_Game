using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

//https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/Actions.html#action-callbacks

public abstract class HeroBase : MonoBehaviour
{
    public HeroAbility abilities;
    private PlayerInput playerInput;
    private Transform projectileSpawnPoint;
    private float projectileSpeed = 20f;

    public float ability1CooldownTimer = 0f;
    public float ability2CooldownTimer = 0f;
    public float ultimateCooldownTimer = 0f;


  

    private AutoAttack autoattack;

    public TMP_Text ability1CooldownText;
    public TMP_Text ability2CooldownText;
    public TMP_Text ultimateCooldownText;

  
    public Image ability1Icon;
    public Image ability2Icon;
    public Image ultimateIcon;

   
    private Color originalAbility1Color;
    private Color originalAbility2Color;
    private Color originalUltimateColor;

    protected virtual void Start()
    {
        // Store the original colors of the icons
        originalAbility1Color = ability1Icon.color;
        originalAbility2Color = ability2Icon.color;
        originalUltimateColor = ultimateIcon.color;

        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Ability1"].performed += ctx => UseAbility1();
        playerInput.actions["Ability2"].performed += ctx => UseAbility2();
        playerInput.actions["Ultimate"].performed += ctx => UseUltimate();

        projectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
        if (!projectileSpawnPoint)
        {
            Debug.LogError($"{gameObject.name} is missing a ProjectileSpawnPoint!");
        }

        autoattack = GetComponentInChildren<AutoAttack>();
        autoattack.InstantiateRevolver(abilities.itemRevolve);
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

    private void Update()
    {
        UpdateCooldowns(); // Update cooldown timers each frame

        // Handle the visuals for the ability icons and cooldown text
        UpdateAbilityUI(ability1CooldownTimer, ability1Icon, ability1CooldownText);
        UpdateAbilityUI(ability2CooldownTimer, ability2Icon, ability2CooldownText);
        UpdateAbilityUI(ultimateCooldownTimer, ultimateIcon, ultimateCooldownText);
    }

    void UpdateCooldowns()
    {
        if (ability1CooldownTimer > 0)
        {
            ability1CooldownTimer -= Time.deltaTime;
        }
        if (ability2CooldownTimer > 0)
        {
            ability2CooldownTimer -= Time.deltaTime;
        }
        if (ultimateCooldownTimer > 0)
        {
            ultimateCooldownTimer -= Time.deltaTime;
        }
    }
    void UpdateAbilityUI(float cooldownTimer, Image abilityIcon, TMP_Text cooldownText)
    {
        if (cooldownTimer > 0)
        {
            // Ability is on cooldown
            abilityIcon.color = Color.gray; // Change the icon color to gray
            cooldownText.gameObject.SetActive(true); // Show the cooldown text
            cooldownText.text = cooldownTimer.ToString(); // Update the cooldown timer text
        }
        else
        {
            // Ability is ready
            abilityIcon.color = originalAbility1Color; // Reset icon color to original
            cooldownText.gameObject.SetActive(false); // Hide the cooldown text
        }
    }

    public void ResetCooldowns()
    {
        ability1CooldownTimer = abilities.ability1.cooldown;
        ability2CooldownTimer = abilities.ability2.cooldown;
        ultimateCooldownTimer = abilities.ultimate.cooldown;
    }

}
