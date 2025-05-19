using UnityEngine;

public class DeadState : BaseState
{
    private float timer = 5f;
    private bool startCount = false;

    public void EnterState(Enemy enemy)
    {
        enemy.isDead = true;
        enemy.DestroyNavMeshAgent();
        enemy.enemySFX.StopSfx();
        enemy.animator.SetBool("death", true);
        Debug.Log("Start Dead");
        enemy.WaitForAnimation(enemy.animator, "10-death_fall_backward", CountingDead);
    }

    public void UpdateState(Enemy enemy)
    {
        if (startCount && timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0)
        {
            GameManager.SpawnEnemy(enemy.waypointSet.set);
            enemy.DestroySelf();
        }
    }

    public void ExitState(Enemy enemy)
    {
        enemy.isDead = false;
        Debug.Log("Stop Dead");
    }

    private void CountingDead()
    {
        startCount = true;
    }
}
