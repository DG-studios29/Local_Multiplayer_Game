using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // Max health a player can have
    public int currentHealth;   // Player's current health

    [Header("UI Elements")]
    public Slider healthSlider; // UI slider to show health visually
    public TMP_Text healthText; // Text element to show exact health values

    private void Awake()
    {
        currentHealth = maxHealth; // Initialize health to max at start
        UpdateHealthUI();          // Update the UI to reflect the initial health
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;                        // Reduce health by damage amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health within valid range
        UpdateHealthUI();                              // Refresh UI

        if (currentHealth <= 0)
        {
            Die(); // Handle player death
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;                       // Increase health by heal amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp health within valid range
        UpdateHealthUI();                              // Refresh UI
    }

    public void UpdateHealthUI()
    {
        if (healthSlider != null)
            healthSlider.value = (float)currentHealth / maxHealth; // Update slider value

        if (healthText != null)
            healthText.text = currentHealth + " / " + maxHealth;  // Update health text
    }

    void Die()
    {
        Debug.Log(gameObject.name + " has died!"); // Log the player death
    }
}
