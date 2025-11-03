using UnityEngine;

public class HealthPack : MonoBehaviour
{
    [Header("Health Pack Settings")]
    [SerializeField] private int healAmount = 25;
    [SerializeField] private bool destroyOnPickup = true;

    private void OnTriggerEnter(Collider other)
    {
        // Check if collided object has PlayerHealth
        PlayerHealth player = other.GetComponent<PlayerHealth>();

        if (player != null)
        {
            // Heal player
            player.Heal(healAmount);

            Debug.Log($"Player healed for {healAmount}. Current health: {player.CurrentHealth}");

            // Destroy health pack
            if (destroyOnPickup)
                Destroy(gameObject);
        }
    }
}
