using UnityEngine;

public static class DrawColliderLine
{
    public static void DrawFOVArcWithLines(Vector3 origin, Vector3 forward, float fovAngle, float viewDistance, Color color, float duration = 0.1f, int segments = 20)
    {
        float halfFOV = fovAngle / 2;
        // Calculate the left and right boundary points of the arc.
        Vector3 leftBoundary = origin + Quaternion.Euler(0, -halfFOV, 0) * forward * viewDistance;
        Vector3 rightBoundary = origin + Quaternion.Euler(0, halfFOV, 0) * forward * viewDistance;

        // Draw lines from the origin to both boundaries.
        Debug.DrawLine(origin, leftBoundary, color, duration);
        Debug.DrawLine(origin, rightBoundary, color, duration);

        // Draw the arc between left and right boundaries.
        float angleStep = fovAngle / segments;
        Vector3 previousPoint = leftBoundary;
        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = -halfFOV + angleStep * i;
            Vector3 nextPoint = origin + Quaternion.Euler(0, currentAngle, 0) * forward * viewDistance;
            Debug.DrawLine(previousPoint, nextPoint, color, duration);
            previousPoint = nextPoint;
        }
    }

    public static void DrawOverlapSphere(Vector3 center, float radius, Color color, float duration = 0.1f, int segments = 24)
    {
        // Draw circle in the XZ plane
        float angleStep = 360f / segments;
        Vector3 prevPoint = center + new Vector3(radius, 0f, 0f);
        for (int i = 1; i <= segments; i++)
        {
            float angle = i * angleStep;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(rad) * radius, 0f, Mathf.Sin(rad) * radius);
            Debug.DrawLine(prevPoint, nextPoint, color, duration);
            prevPoint = nextPoint;
        }
    }

    public static void DrawRaycastTarget(Vector3 position, Vector3 direction, float distance, Color color)
    {
        Debug.DrawRay(position, direction * distance, color, 0.1f);
    }
}