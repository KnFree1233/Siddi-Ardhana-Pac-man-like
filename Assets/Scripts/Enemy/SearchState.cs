using UnityEngine;

public class SearchState : BaseState
{
    private bool isMoving;
    public Vector3 targetPosition;
    public int searchedPlaceCount;
    private float delayTimer;
    private Vector3 lookDirection;

    public void EnterState(Enemy enemy)
    {
        delayTimer = 0;
        searchedPlaceCount = 0;
        enemy.navMeshAgent.speed = enemy.normalSpeed;

        if (enemy.detectingPlayer.currTarget != null) targetPosition = enemy.detectingPlayer.currTarget.position;
        else targetPosition = enemy.player.transform.position;

        lookDirection = targetPosition - enemy.transform.position;
        enemy.navMeshAgent.destination = targetPosition;
        isMoving = true;
        Debug.Log("Start Searching");
    }

    public void UpdateState(Enemy enemy)
    {
        // DrawColliderLine.DrawOverlapSphere(targetPosition, enemy.searchRadius * searchedPlaceCount, Color.yellow);
        if (enemy.player.isInvisible)
        {
            enemy.SwitchState(enemy.patrolState);
        }
        else if(!enemy.player.isInvisible)
        {
            if (enemy.detectingPlayer.VisionOnPlayer(enemy))
            {
                enemy.SwitchState(enemy.chaseState);
                return;
            }
            if (enemy.detectingPlayer.HearingPlayer(enemy))
            {
                searchedPlaceCount = 0;
                delayTimer = 0;
                isMoving = true;
                enemy.navMeshAgent.destination = enemy.detectingPlayer.currTarget.position;
            }
        }
        if (searchedPlaceCount >= enemy.manyPlaceToSearch)
        {
            enemy.SwitchState(enemy.patrolState);
        }
        if (!isMoving)
        {
            if (delayTimer < enemy.searchMoveDelay)
            {
                //Enemy Look Around base on angleRotate
                float angleOffset = Mathf.PingPong(Time.time * enemy.rotatingSpeed, enemy.angleRotate * 2) - enemy.angleRotate;
                Vector3 newDirection = Quaternion.Euler(0, angleOffset, 0) * lookDirection;
                enemy.transform.rotation = Quaternion.Slerp(
                    enemy.transform.rotation,
                    Quaternion.LookRotation(newDirection),
                    Time.deltaTime * enemy.rotatingSpeed
                );

                delayTimer += Time.deltaTime;
            }
            else if (delayTimer >= enemy.searchMoveDelay)
            {
                searchedPlaceCount++;
                isMoving = true;
                Vector3 searchingArea = Random.insideUnitSphere * enemy.searchRadius * searchedPlaceCount;
                searchingArea.y = 0;
                Vector3 destination = targetPosition + searchingArea;
                lookDirection = destination - enemy.transform.position;
                enemy.navMeshAgent.destination = destination;
            }
        }
        else
        {
            if (!enemy.navMeshAgent.pathPending && enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
            {
                delayTimer = 0;
                isMoving = false;
            }
        }
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Searching");
    }
}
