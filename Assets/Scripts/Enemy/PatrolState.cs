using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    private bool isMoving;
    Vector3 destination;

    public void EnterState(Enemy enemy)
    {
        enemy.navMeshAgent.speed = enemy.normalSpeed;
        isMoving = false;
        Debug.Log("Start Patrol");
    }

    public void UpdateState(Enemy enemy)
    {
        if (DetectingPlayer.DetectPlayer(enemy))
        {
            enemy.SwitchState(enemy.chaseState);
        }
        if (!isMoving)
        {
            isMoving = true;
            int index = Random.Range(0, enemy.waypoints.Count);
            destination = enemy.waypoints[index].position;
            enemy.navMeshAgent.destination = destination;
        }
        else
        {
            if (Vector3.Distance(destination, enemy.transform.position) <= 0.1)
            {
                isMoving = false;
            }
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Patrol");
    }
}
