using UnityEngine;

public class PatrolState : BaseState
{
    private bool isMoving;
    private Vector3 destination;

    public void EnterState(Enemy enemy)
    {
        enemy.navMeshAgent.speed = enemy.normalSpeed;
        isMoving = false;
        Debug.Log("Start Patrol");
    }

    public void UpdateState(Enemy enemy)
    {
        if (!enemy.player.isInvisible)
        {
            if (enemy.detectingPlayer.VisionOnPlayer(enemy))
            {
                enemy.SwitchState(enemy.chaseState);
            }
            else if (enemy.detectingPlayer.HearingPlayer(enemy))
            {
                enemy.SwitchState(enemy.searchState);
            }
        }
        if (!isMoving)
        {
            isMoving = true;
            int index = Random.Range(0, enemy.waypointSet.waypoints.Count);
            destination = enemy.waypointSet.waypoints[index].position;

            enemy.navMeshAgent.destination = destination;
        }
        else
        {
            if (!enemy.navMeshAgent.pathPending && enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
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
