using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculate : MonoBehaviour
{
    public static float PositionToRadian(Vector3 position, Vector3 targetPosition)
    {
        float radian = Mathf.Atan2(targetPosition.y - position.y, targetPosition.x - position.x);
        if (radian < 0)
        {
            radian = radian + 2 * Mathf.PI;
        }

        return radian;
    }

    public static Vector3 PositionToNomaliseVector(Vector3 position, Vector3 targetPosition)
    {
        float radian = PositionToRadian(position, targetPosition);
        float distance = 1.0f;

        return new Vector3(Mathf.Cos(radian) * distance, Mathf.Sin(radian) * distance, 0);
    }
}
