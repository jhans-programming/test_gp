using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy enemy;               // Reference to Enemy script
    [SerializeField] private Image foregroundImage;     // Foreground image that fills/reduces
    [SerializeField] private Image backgroundImage;     // Optional background (for visuals)

    [Header("Smooth Animation")]
    [SerializeField] private float fillSpeed = 5f;      // Controls how smoothly the bar updates

    private float targetFillAmount;

    private void Start()
    {
        if (enemy == null)
        {
            // Try to auto-assign if on same GameObject
            enemy = GetComponentInParent<Enemy>();
            if (enemy == null)
            {
                Debug.LogError("HealthBarUI: Enemy reference not assigned!");
                enabled = false;
                return;
            }
        }

        if (foregroundImage == null)
        {
            Debug.LogError("HealthBarUI: Foreground Image not assigned!");
            enabled = false;
            return;
        }

        // Initialize health bar
        targetFillAmount = 1f;
        foregroundImage.fillAmount = targetFillAmount;
    }

    private void Update()
    {
        if (enemy == null) return;

        // Calculate target fill based on enemy health
        float currentHealth = Mathf.Max(0f, enemyHealth());
        targetFillAmount = currentHealth / enemyMaxHealth();

        // Smoothly interpolate between current fill and target fill
        foregroundImage.fillAmount = Mathf.Lerp(
            foregroundImage.fillAmount,
            targetFillAmount,
            Time.deltaTime * fillSpeed
        );
    }

    private float enemyHealth()
    {
        // Access the protected field "health" using reflection since it's protected
        var field = typeof(Enemy).GetField("health", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (float)field.GetValue(enemy);
    }

    private float enemyMaxHealth()
    {
        var field = typeof(Enemy).GetField("maxHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (float)field.GetValue(enemy);
    }
}
