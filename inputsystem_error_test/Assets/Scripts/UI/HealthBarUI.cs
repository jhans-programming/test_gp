using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth; // Reference to PlayerHealth script
    [SerializeField] private Image foregroundImage;     // Foreground image that fills/reduces
    [SerializeField] private Image backgroundImage;     // Optional background (for visuals)

    [Header("Smooth Animation")]
    [SerializeField] private float fillSpeed = 5f;      // Controls how smoothly the bar updates

    private float targetFillAmount;

    private void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("HealthBarUI: PlayerHealth reference not assigned!");
            enabled = false;
            return;
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
        // Calculate target fill based on player health
        targetFillAmount = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;

        // Smoothly interpolate between current fill and target fill
        foregroundImage.fillAmount = Mathf.Lerp(foregroundImage.fillAmount, targetFillAmount, Time.deltaTime * fillSpeed);
    }
}
