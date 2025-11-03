using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : Enemy
{
    [Header("Enemy Shooter Shooting Config")]
    [SerializeField] private float startShootDistance = 10f;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootInterval = 1f;
    [SerializeField] private Transform shootPoint;
    private float lastShootTime;
    [Header("Enemy Shooter Fallback Config")]
    [SerializeField] private float startFallBackDistance = 7f; // Must always be < startShootDistance
    [SerializeField] private float fallBackSpeed = 2f;

    protected override void Awake()
    {
        base.Awake();
        lastShootTime = 0;
    }

    protected override void DoEnemyAI()
    {
        base.DoEnemyAI();

        // Calc distance to player
        float d = Vector3.Distance(transform.position, player.position);
        
        if (d < startShootDistance)
        {
            Shoot();
            
            if (d < startFallBackDistance)
            {
                // Destination = a point behind the enemy
                Vector3 fallBackDest = transform.position - dirToPlayer;
                agent.SetDestination(fallBackDest);

                // While falling back, keep facing the player
                agent.updateRotation = false;
                transform.LookAt(player);

            } 
            else
            {
                // If player not too close, don't move
                agent.ResetPath();
                transform.LookAt(player);
            }
        } 
        else
        {
            agent.updateRotation = true;
            agent.SetDestination(player.position);
        }
    }

    private void Shoot()
    {
        if (Time.time - lastShootTime >= shootInterval)
        {
            Instantiate(projectile, shootPoint.position, Quaternion.LookRotation(dirToPlayer));
            lastShootTime = Time.time;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, startShootDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, startFallBackDistance);
    }
}
