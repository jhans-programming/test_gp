using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    public AudioClip PlayerHurt;
    // Property to expose read-only current health
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    // void Start()
    // {
    //     // Initialize current health to the maximum
    //     currentHealth = maxHealth;
    // }

    void Awake()
{
    // ✅ Only set currentHealth if not already set by GameManager
    if (currentHealth <= 0)
        currentHealth = maxHealth;
}

    public void Update()
    {
        Debug.Log("my health is" + currentHealth);
    }

    // Take damage dynamically
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        AudioManager.Instance.PlaySFX(PlayerHurt);
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

    public void SetCurrentHealth(int newHealth)
    {
        currentHealth = newHealth;
        Debug.Log($"Player current health set to {currentHealth}");
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"Player healed {amount}. Current health: {currentHealth}");
    }

    private void Die()
    {
        Debug.Log("Player has died!");

        GameManager.Instance.ShowLoseScreen();

        // Optional: disable player controls/movement here
        Destroy(gameObject, 0.2f); // delay so the death event runs
    }
}
