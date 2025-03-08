using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SearchState : BaseState
{
    private bool isMoving;
    private Vector3 targetPosition;
    private int searchedPlaceCount;
    private float delayTimer;

    public void EnterState(Enemy enemy)
    {
        delayTimer = 0;
        enemy.animator.SetTrigger("SearchState");
        searchedPlaceCount = 0;
        enemy.navMeshAgent.speed = enemy.normalSpeed;
        isMoving = false;
        targetPosition = enemy.player.transform.position;
        Debug.Log("Start Searching");
    }

    public void UpdateState(Enemy enemy)
    {
        if (DetectingPlayer.DetectPlayer(enemy) && !enemy.player.isInvisible)
        {
            enemy.SwitchState(enemy.chaseState);
        }
        if (!isMoving && delayTimer < enemy.searchMoveDelay)
        {
            delayTimer += Time.deltaTime;
        }
        else if (searchedPlaceCount >= enemy.manyPlaceToSearch)
        {
            enemy.SwitchState(enemy.patrolState);
        }
        else if (!isMoving && delayTimer >= enemy.searchMoveDelay)
        {
            enemy.audioSource.Play();
            isMoving = true;
            Vector3 searchingArea = Random.insideUnitSphere * enemy.searchRadius;
            searchingArea.y = 0;
            Vector3 destination = targetPosition + searchingArea;
            enemy.navMeshAgent.destination = destination;
            searchedPlaceCount++;
        }
        else
        {
            if (!enemy.navMeshAgent.pathPending && enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
            {
                enemy.audioSource.Stop();
                delayTimer = 0;
                isMoving = false;
            }
        }
    }

    public void ExitState(Enemy enemy)
    {
        enemy.audioSource.Play();
        Debug.Log("Stop Searching");
    }
}
