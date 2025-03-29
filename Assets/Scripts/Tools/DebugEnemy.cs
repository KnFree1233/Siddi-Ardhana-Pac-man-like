using UnityEngine;

public class DebugEnemy : MonoBehaviour
{
    private Enemy enemy;
    private Player player;

    public void Init(Enemy enemy, Player player)
    {
        this.enemy = enemy;
        this.player = player;
    }

    private void Update()
    {
        DebugOnSceneScreen();
    }

    private void DebugOnSceneScreen()
    {
        //Field of View Enemy
        DrawColliderLine.DrawFOVArcWithLines(transform.position, transform.forward, enemy.fovAngle, enemy.radius, Color.red, 0.1f, 20);
        //Enemy Range Hearing Player Walk Noise
        DrawColliderLine.DrawOverlapSphere(transform.position, enemy.noiseTolerance + player.GetWalkNoise(), Color.cyan, 0.1f);
        //Enemy Range Hearing Player Run Noise
        DrawColliderLine.DrawOverlapSphere(transform.position, enemy.noiseTolerance + player.GetRunNoise(), Color.blue, 0.1f);

        //Raycast When Player on Enemy View
        if (enemy.detectingPlayer.VisionOnPlayer(enemy))
        {
            Vector3 directionToTarget = (player.transform.position - enemy.transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
            DrawColliderLine.DrawRaycastTarget(transform.position, directionToTarget, distanceToTarget, Color.green);
        }
    }
}
