using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnDeath; // Event for death notification

    [Header("Base Stats")]
    [SerializeField] protected float maxHealth = 50f;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float attackDamage = 5f;

    protected float health;
    protected NavMeshAgent agent;
    protected Transform player;
    protected Vector3 dirToPlayer;

    protected virtual void Awake()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        dirToPlayer = (player.position - transform.position).normalized;
        DoEnemyAI();
    }

    protected virtual void DoEnemyAI() { }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log($"{gameObject.name} was hit! Damage: {dmg}, Remaining Health: {health}");

        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has died!");

        // Trigger death event
        OnDeath?.Invoke(this);

        Destroy(gameObject);
    }
}
