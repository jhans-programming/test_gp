using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnDeath;

    [Header("Base Stats")]
    [SerializeField] protected float maxHealth = 50f;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float attackDamage = 5f;

    private Color originalColor;
    private Renderer rend;

    protected float health;
    protected NavMeshAgent agent;
    protected Transform player;
    protected Vector3 dirToPlayer;

    //private Coroutine burnCoroutine;
    //private Coroutine slowCoroutine;

    protected virtual void Awake()
    {
        health = maxHealth;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        player = GameObject.FindWithTag("Player").transform;

        // --- Get Renderer and clone material so we do not modify prefab ---
        rend = GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material = new Material(rend.material);
            originalColor = rend.material.color;
        }
    }

    void Update()
    {
        if (player == null) 
            return; // stop AI because player is gone

        dirToPlayer = (player.position - transform.position).normalized;
        DoEnemyAI();
    }

    protected virtual void DoEnemyAI() { }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0)
            Die();
    }

    protected virtual void Die()
    {
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    /*
    // ----------------- SLOW EFFECT -----------------
    public void ApplySlow(float slowAmount, float duration)
    {
        if (slowCoroutine != null) StopCoroutine(slowCoroutine);
        slowCoroutine = StartCoroutine(SlowCoroutine(slowAmount, duration));
    }

    private IEnumerator SlowCoroutine(float slowAmount, float duration)
    {
        float originalSpeed = agent.speed;

        agent.speed = originalSpeed * (1 - slowAmount);

        // Tint BLUE for slow
        if (rend != null)
            rend.material.color = new Color(0.5f, 0.5f, 1f, 1f);

        yield return new WaitForSeconds(duration);

        agent.speed = originalSpeed;

        // Restore color only if no burn is active
        rend.material.color = originalColor;
    }
    */

    // ----------------- BURN EFFECT -----------------
    /*
    public void ApplyBurn(float damagePerTick, float duration, float tickInterval = 1f)
    {
        if (burnCoroutine != null) StopCoroutine(burnCoroutine);
        burnCoroutine = StartCoroutine(BurnCoroutine(damagePerTick, duration, tickInterval));
    }

    private IEnumerator BurnCoroutine(float damagePerTick, float duration, float tickInterval)
    {
        float elapsed = 0f;

        // Tint YELLOW/ORANGE
        if (rend != null)
            rend.material.color = new Color(1f, 0.85f, 0.2f, 1f);

        while (elapsed < duration)
        {
            TakeDamage(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        // Restore original color only if no slow is active
        rend.material.color = originalColor;
    }
    */
}
