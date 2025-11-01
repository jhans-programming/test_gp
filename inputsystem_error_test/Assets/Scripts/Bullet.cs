using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 2f;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lifetime = 10f;
    private float timeCreated;

    private void Awake()
    {
        timeCreated = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += speed * Time.deltaTime * transform.forward;
        if (Time.time - timeCreated >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // --- Handle Player hit ---
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage((int)damage);
                Debug.Log($"Player hit! Took {damage} damage.");
            }
            Destroy(gameObject);
            return;
        }

        // --- Handle Enemy hit ---
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Enemy took {damage} damage from bullet!");
            }

            Destroy(gameObject);
            return;
        }
    }
}
