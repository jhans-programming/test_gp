using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRam : Enemy
{
    [Header("Enemy Ram Stats")]
    [SerializeField] private float prepareRamDistance = 8f;
    [SerializeField] private float ramSpeed = 6f;
    [SerializeField] private float prepareRamTime = 1.5f;
    [SerializeField] private float recoveryTime = 1.5f;

    private float startPrepareRamTime;
    private float startRamTime;
    private float startRecoveryTime;
    private bool isRamming;

    private enum RamState
    {
        Preparing,
        Executing,
        Recovering
    }

    private RamState ramState;

    protected override void Awake()
    {
        base.Awake();
        isRamming = false;
        startPrepareRamTime = 0;
        startRecoveryTime = 0;
    }

    protected override void DoEnemyAI()
    {
        base.DoEnemyAI();

        // Calc dist to player
        float d = Vector3.Distance(transform.position, player.position);

        if (!isRamming)
        {
            agent.speed = moveSpeed;

            // Get close to player
            if (d <= prepareRamDistance && Time.time - startRecoveryTime > recoveryTime)
            {
                // Start prepare ram
                startPrepareRamTime = Time.time;
                isRamming = true;
                ramState = RamState.Preparing;
            }
            else
            {
                agent.SetDestination(player.position);
            }
        } 
        else
        {
            Debug.Log(ramState);

            if (ramState == RamState.Preparing)
            {
                if (d > prepareRamDistance)
                {
                    // If player gets too far while preparing ram, cancel ram
                    isRamming = false;
                    return;
                }

                agent.ResetPath();
                transform.LookAt(player);

                if (Time.time - startPrepareRamTime >= prepareRamTime)
                {
                    // Transition to RamState.Executing
                    startRamTime = Time.time;
                    ramState = RamState.Executing;

                    // Set destination to a far-away point behind the player
                    agent.SetDestination(player.position);
                }
            } 
            else if (ramState == RamState.Executing)
            {
                agent.speed = ramSpeed;

                if (agent.remainingDistance < 0.1f)
                {
                    // Transition to RamState.Recovering
                    isRamming = false;
                    startRecoveryTime = Time.time;
                } 
            } 
        }
        
    }
}
