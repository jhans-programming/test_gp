using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletEffect { None, Slow, Burn }
    public enum ShooterTag { Player, Enemy}

    [SerializeField] private float damage = 2f;
    [SerializeField] private float speed = 100f;
    [SerializeField] private float lifetime = 10f;

    [SerializeField] private BulletEffect effect = BulletEffect.None; // private but assignable

    private float timeCreated;

    [System.NonSerialized] public ShooterTag shooterTag;

    private void Awake()
    {
        timeCreated = Time.time;
    }

    public void SetEffect(BulletEffect newEffect)
    {
        effect = newEffect;
    }

    void FixedUpdate()
    {
        transform.position += speed * Time.deltaTime * transform.forward;

        if (Time.time - timeCreated >= lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // --- Player hit ---
        if (other.CompareTag("Player") && shooterTag == ShooterTag.Enemy)
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage((int)damage);

            Destroy(gameObject);
            return;
        }

        // --- Enemy hit ---
        if (other.CompareTag("Enemy") && shooterTag == ShooterTag.Player)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                /*
                switch (effect)
                {
                    case BulletEffect.Slow:
                        enemy.ApplySlow(0.5f, 2f); // You can tweak these values
                        break;

                    case BulletEffect.Burn:
                        enemy.ApplyBurn(1f, 4f); // damagePerTick, duration
                        break;
                }
                */
            }

            Destroy(gameObject);
        }
    }
}
