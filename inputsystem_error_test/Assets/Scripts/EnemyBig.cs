using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBig : Enemy
{
    [Header("Enemy Big Stats")]
    [SerializeField] private GameObject childrenEnemy;
    [SerializeField] private int childCount = 3;

    protected override void DoEnemyAI()
    {
        base.DoEnemyAI();
        agent.SetDestination(player.position);

        // DEBUG PURPOSES ONLY, DELETE AFTER!
        if (Input.GetKeyDown(KeyCode.P))
        {
            Die();
        }
    }

    protected override void Die()
    {
        for (int i = 0; i < childCount; i++)
        {
            Instantiate(childrenEnemy, transform.position, Quaternion.LookRotation(dirToPlayer));
        }
        base.Die();
    }
}
