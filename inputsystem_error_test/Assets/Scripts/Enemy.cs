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
    [SerializeField] private float maxHealth = 50f;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackDamage = 5f;

    protected float health;
    protected NavMeshAgent agent;
    protected Transform player;

    protected virtual void Awake()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
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

    private void Die()
    {
        // TODO: Spawn destroyed particle effect
        Destroy(gameObject);
    }
}
