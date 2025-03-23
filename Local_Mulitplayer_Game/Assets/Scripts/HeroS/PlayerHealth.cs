using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.AI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("UI Elements")]
    public Slider healthSlider;
    public TMP_Text healthText;

    private bool isFrozen;
    private float freezeDuration;
    private void Awake()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI(); // 🔹 Ensure UI updates every time damage is taken

        Debug.Log($"{gameObject.name} took {damage} damage. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = (float)currentHealth / maxHealth; // 🔹 Ensure slider updates

        if (healthText != null)
            healthText.text = $"{currentHealth} / {maxHealth}"; // 🔹 Ensure text updates
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        // 🔹 You can add respawn logic or player death effects here
    }

    public void Freeze(float duration)
    {
        if (isFrozen) return; // Prevent freezing if already frozen
        isFrozen = true;
        freezeDuration = duration;

        StartCoroutine(FreezeDuration());
    }

    private IEnumerator FreezeDuration()
    {
        yield return new WaitForSeconds(freezeDuration);
        isFrozen = false;
    }
}
