using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 20;   // How much damage the spikes deal
    
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has a PlayerHealth component
        PlayerHealth health = other.GetComponent<PlayerHealth>();

        if (health != null)
        {
            // Apply damage
            health.TakeDamage(damageAmount);
            Debug.Log($"{other.name} took {damageAmount} damage from spikes!");
        }
    }
}
