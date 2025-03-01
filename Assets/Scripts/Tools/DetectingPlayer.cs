using UnityEngine;

public static class DetectingPlayer
{
    //Function for detect player based on angle fov and enemy can't see through wall
    public static bool DetectPlayer(Enemy enemy)
    {
        Transform enemyTransform = enemy.transform;

        Collider[] rangeChecks = Physics.OverlapSphere(enemyTransform.position, enemy.radius, enemy.playerLayer);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - enemyTransform.position).normalized;

            if (Vector3.Angle(enemyTransform.forward, directionToTarget) < enemy.fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(enemyTransform.position, target.position);
                if (!Physics.Raycast(enemyTransform.position, directionToTarget, distanceToTarget, enemy.obstacleLayer))
                {
                    Debug.DrawRay(enemyTransform.position, directionToTarget * enemy.radius, Color.green, 0.1f);
                    return true;
                }
                else
                {
                    Debug.DrawRay(enemyTransform.position, directionToTarget * enemy.radius, Color.red, 0.1f);
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
        else
        {
            return false;
        }
    }
}