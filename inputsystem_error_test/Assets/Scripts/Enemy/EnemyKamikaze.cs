using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKamikaze : Enemy
{
    protected override void DoEnemyAI()
    {
        base.DoEnemyAI();
        agent.SetDestination(player.position);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage((int)attackDamage);
            Die();
        }
    }
}
