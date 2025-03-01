using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState
{
    private float playerLostTime;

    public void EnterState(Enemy enemy)
    {
        playerLostTime = 0;
        enemy.navMeshAgent.speed = enemy.chaseSpeed;
        Debug.Log("Start Chasing");
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.player != null)
        {
            enemy.navMeshAgent.destination = enemy.player.transform.position;
            if (DetectingPlayer.DetectPlayer(enemy))
            {
                playerLostTime = 0;
            }
            if (playerLostTime >= enemy.timeToLostPlayer)
            {
                enemy.SwitchState(enemy.patrolState);
            }
        }

        playerLostTime += Time.deltaTime;
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Chasing");
    }
}
