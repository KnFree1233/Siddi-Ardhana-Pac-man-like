using UnityEngine;

public static class DetectingPlayer
{
    //Function for detect player based on angle fov and enemy can't see through wall
    public static bool DetectPlayer(Enemy enemy)
    {
        Transform enemyTransform = enemy.transform;
        Collider[] rangeChecks = CheckOverlapShpere(enemyTransform.position, enemy.radius, enemy.playerLayer);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - enemyTransform.position).normalized;

            if (Vector3.Angle(enemyTransform.forward, directionToTarget) < enemy.fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(enemyTransform.position, target.position);
                if (!Physics.Raycast(enemyTransform.position, directionToTarget, distanceToTarget, enemy.obstacleLayer))
                {
                    DrawColliderLine.DrawRaycastTarget(enemyTransform.position, directionToTarget, enemy.radius, Color.green);
                    return true;
                }
                else
                {
                    DrawColliderLine.DrawRaycastTarget(enemyTransform.position, directionToTarget, enemy.radius, Color.red);
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

    //Function for detect player base float noise on player and float noise tolerance on enemy
    public static bool HearingPlayer(Enemy enemy)
    {
        enemy.target = null;
        Transform enemyTransform = enemy.transform;
        Collider[] rangeChecks = CheckOverlapShpere(enemyTransform.position, enemy.radius, enemy.playerLayer);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            float distanceToTarget = Vector3.Distance(target.position, enemyTransform.position);

            if (distanceToTarget - enemy.player.currNoise < enemy.noiseTolerance)
            {
                enemy.target = target;
                return true;
            }
        }

        return false;
    }

    private static Collider[] CheckOverlapShpere(Vector3 position, float radius, LayerMask layer)
    {
        return Physics.OverlapSphere(position, radius, layer);
    }
}