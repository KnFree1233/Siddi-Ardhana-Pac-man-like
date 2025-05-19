using UnityEngine;

public class DebugEnemy : MonoBehaviour
{
    private Enemy enemy;
    private Player player;
    private Vector3 targetPosition;
    private SearchState searchState;

    public void Init(Enemy enemy, Player player)
    {
        this.enemy = enemy;
        this.player = player;
        targetPosition = Vector3.zero;
        searchState = null;
    }

    private void Update()
    {
        DebugOnSceneScreen();
    }

    private void DebugOnSceneScreen()
    {
        DetectingPlayer detectingPlayer = enemy.detectingPlayer;

        //Field of View Enemy
        DrawColliderLine.DrawFOVArcWithLines(transform.position, transform.forward, detectingPlayer.fovAngle, detectingPlayer.radius, Color.red, 0.1f, 20);
        //Enemy Range Hearing Player Walk Noise
        DrawColliderLine.DrawOverlapSphere(transform.position, detectingPlayer.noiseTolerance + player.GetWalkNoise(), Color.cyan, 0.1f);
        //Enemy Range Hearing Player Run Noise
        DrawColliderLine.DrawOverlapSphere(transform.position, detectingPlayer.noiseTolerance + player.GetRunNoise(), Color.blue, 0.1f);

        //Raycast When Player on Enemy View
        if (enemy.detectingPlayer.VisionOnPlayer(enemy))
        {
            Vector3 directionToTarget = (player.transform.position - enemy.transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, player.transform.position);
            DrawColliderLine.DrawRaycastTarget(transform.position, directionToTarget, distanceToTarget, Color.green);
        }
        if (enemy.currentState.GetType() == typeof(SearchState))
        {
            if (targetPosition == Vector3.zero && searchState == null)
            {
                searchState = enemy.currentState as SearchState;
                targetPosition = searchState.targetPosition;
            }
            DrawColliderLine.DrawOverlapSphere(targetPosition, enemy.searchRadius * (searchState.searchedPlaceCount + 1), Color.yellow);
            if (enemy.manyPlaceToSearch >= searchState.searchedPlaceCount)
            {
                targetPosition = Vector3.zero;
                searchState = null;
            }
        }
    }
}
