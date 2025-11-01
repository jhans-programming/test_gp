using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    // Property to expose read-only current health
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    void Start()
    {
        // Initialize current health to the maximum
        currentHealth = maxHealth;
    }

    // Take damage dynamically
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player took {amount} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Change the max health dynamically at runtime
    public void SetMaxHealth(int newMaxHealth, bool adjustCurrent = true)
    {
        maxHealth = Mathf.Max(1, newMaxHealth);
        if (adjustCurrent)
            currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"Max health set to {maxHealth}. Current health: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        Destroy(gameObject);
    }
}
