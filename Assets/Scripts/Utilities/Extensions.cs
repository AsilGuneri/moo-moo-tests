using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class Extensions : MonoBehaviour
{
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }
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
        {
            return hit.position;

        }
        else
        {
            return clickPos;

        }

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

    public static bool CheckRange(Vector3 unitPos, Vector3 targetPos, float range){
        return GetDistance(unitPos, targetPos) <= range;
    }
    public static bool CheckRangeBetweenUnits(Transform unit, Transform target, float range)
    {
        float distanceBetweenCenters = GetDistance(unit.position, target.position);
        var unitRadius = unit.GetComponent<UnitController>().Movement.AgentRadius;
        var targetRadius = target.GetComponent<UnitController>().Movement.AgentRadius;

        float distanceBetweenEdges = distanceBetweenCenters - (unitRadius + targetRadius);
        return distanceBetweenEdges <= range;
    }
    public static bool CheckRangeUnitAndCollider3D(UnitController unit, Collider target, float range)
    {
        float distanceBetweenCenters = GetDistance(unit.transform.position, target.transform.position);
        var unitRadius = unit.Movement.AgentRadius;
        var targetRadius = GetMaxColliderDimension(target);
        targetRadius = targetRadius == -1 ? 0.5f : targetRadius;
        float distanceBetweenEdges = distanceBetweenCenters - (unitRadius + targetRadius);
        return distanceBetweenEdges <= range;
    }
    public static float GetDistance(Vector3 currentTargetPosition, Vector3 currentUnitPosition){
        return Vector2.Distance(Extensions.To2D(currentTargetPosition), Extensions.To2D(currentUnitPosition));
    }
    /// <summary>
    /// Time in miliseconds.
    /// </summary>
    /// <param name="attackSpeed"></param>
    /// <param name="triggerPointOfAnim"></param>
    /// <returns></returns>
    public static void GetAttackTimes(float animLength, float attackSpeed, float triggerPointOfAnim, out int msBeforeAttack, out int msAfterAttack)
    {
         msBeforeAttack = Extensions.ToMiliSeconds(((animLength / attackSpeed) * triggerPointOfAnim));
         msAfterAttack = Extensions.ToMiliSeconds((animLength / attackSpeed) * (1 - triggerPointOfAnim));

    }

    public static float GetMaxColliderDimension(Collider col)
    {
        if (col is BoxCollider box)
        {
            return Mathf.Max(box.size.x, box.size.y, box.size.z);
        }
        else if (col is SphereCollider sphere)
        {
            return sphere.radius * 2f;  // Diameter
        }
        else if (col is CapsuleCollider capsule)
        {
            // For CapsuleCollider, we should consider both its height and diameter (2 * radius).
            return Mathf.Max(capsule.height, 2f * capsule.radius);
        }
        // Handle other collider types as needed.
        // Returning -1 to indicate an unsupported collider type.
        return -1f;
    }

}
