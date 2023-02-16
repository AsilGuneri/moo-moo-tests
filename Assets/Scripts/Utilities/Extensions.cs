using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Extensions : MonoBehaviour
{
    public static Vector2 Vector3ToVector2(Vector3 initialVector)
    {
        Vector2 finalVector = new Vector2(initialVector.x, initialVector.z);
        return finalVector;
    }
    public static float Distance(Vector3 pointA, Vector3 pointB)
    {
        return Vector2.Distance(Vector3ToVector2(pointA), Vector3ToVector2(pointB));
    }
    public static bool IsInRange(Vector3 pointA, Vector3 pointB, float range)
    {
        return Distance(pointA, pointB) <= range;
    }
    public static int ToMiliSeconds(float seconds)
    {
        if (seconds == 0) return 0;
        int miliSeconds = (int)(seconds * 1000);
        return miliSeconds;
    }
}
