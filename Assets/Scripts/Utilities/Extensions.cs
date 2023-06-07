using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class Extensions : MonoBehaviour
{
    public static Vector2 To2D(Vector3 initialVector)
    {
        Vector2 finalVector = new Vector2(initialVector.x, initialVector.z);
        return finalVector;
    }
    public static Vector3 Vector3WithoutY(Vector3 vector)
    {
        Vector3 newVector = new Vector3(vector.x, 0, vector.z);
        return newVector;
    }
    public static float Distance(Vector3 pointA, Vector3 pointB)
    {
        return Vector2.Distance(To2D(pointA), To2D(pointB));
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
    public static Vector3 CheckNavMesh(Vector3 clickPos)
    {
        NavMeshHit hit;

        // Check for nearest point on navmesh within a certain range (here 5 units)
        if (NavMesh.SamplePosition(clickPos, out hit, 15.0f, NavMesh.AllAreas))
            return hit.position;
        else
            return clickPos;
        
    }
    public static Vector3 GetMouseHitPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            var hitPoint = hit.point;
            return hitPoint;
        }
        return Vector3.zero;
    }
}
