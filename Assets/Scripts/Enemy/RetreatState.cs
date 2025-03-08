using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetreatState : BaseState
{
    public void EnterState(Enemy enemy)
    {
        enemy.animator.SetTrigger("RetreatState");
        enemy.navMeshAgent.speed = enemy.chaseSpeed;
        Debug.Log("Start Retreating");
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.player != null)
        {
            enemy.navMeshAgent.destination = enemy.transform.position + (enemy.transform.position - enemy.player.transform.position);
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Retreating");
    }
}
