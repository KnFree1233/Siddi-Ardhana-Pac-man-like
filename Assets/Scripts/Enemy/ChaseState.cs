using UnityEngine;

public class ChaseState : BaseState
{
    private float playerLostTime;

    public void EnterState(Enemy enemy)
    {
        SfxManager.Instance.PlayAudio("Found Player");
        playerLostTime = 0;
        enemy.navMeshAgent.speed = enemy.chaseSpeed;
        Debug.Log("Start Chasing");
    }

    public void UpdateState(Enemy enemy)
    {
        if (enemy.player != null)
        {
            if (enemy.player.isInvisible)
            {
                enemy.SwitchState(enemy.patrolState);
                return;
            }
            if (enemy.detectingPlayer.VisionOnPlayer(enemy))
            {
                playerLostTime = 0;
                enemy.navMeshAgent.destination = enemy.player.transform.position;
                return;
            }
            else if (enemy.detectingPlayer.HearingPlayer(enemy))
            {
                playerLostTime = 0;
                enemy.navMeshAgent.destination = enemy.detectingPlayer.currTarget.position;
                return;
            }
            if (playerLostTime >= enemy.timeToLostPlayer)
            {
                enemy.SwitchState(enemy.searchState);
            }
        }
        playerLostTime += Time.deltaTime;
    }

    public void ExitState(Enemy enemy)
    {
        Debug.Log("Stop Chasing");
    }
}
