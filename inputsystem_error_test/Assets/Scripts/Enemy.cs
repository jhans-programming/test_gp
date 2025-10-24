using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    // Base Enemy class
    // - Enemy has health
    // - Enemy can take damage, can die
    // - Enemy can attack
    // - 

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

    // Update is called once per frame
    void Update()
    {
        dirToPlayer = (player.position - transform.position).normalized;
        DoEnemyAI();
    }

    protected virtual void DoEnemyAI()
    {

    }

    protected void TakeDamage(float dmg)
    {
        health -= dmg;
        // TODO: Play hurt animation, hurt sound
        
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        // TODO: Spawn destroyed particle effect
        Destroy(gameObject);
    }
}
