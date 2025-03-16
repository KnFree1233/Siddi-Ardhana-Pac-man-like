using UnityEngine;

public class DeadState : BaseState
{
    private float delayTimer;

    public void EnterState(Enemy enemy)
    {
        enemy.isDead = true;
        enemy.audioSource.Stop();
        delayTimer = 0;
        enemy.animator.SetTrigger("DeadState");
        Debug.Log("Start Dead");
    }

    public void UpdateState(Enemy enemy)
    {
        if (delayTimer >= enemy.deadDelay)
        {
            enemy.SwitchState(enemy.patrolState);
        }
        delayTimer += Time.deltaTime;
    }

    public void ExitState(Enemy enemy)
    {
        enemy.isDead = false;
        enemy.audioSource.Play();
        Debug.Log("Stop Dead");
    }
}
