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
        Debug.Log($"asilxx0 {NavMesh.GetAreaFromName("Walkable")} {NavMesh.GetAreaFromName("Unwalkable")}");
        // Check for nearest point on navmesh within a certain range (here 5 units)
        if (NavMesh.SamplePosition(clickPos, out hit, 15.0f, NavMesh.AllAreas))
        {
            Debug.Log($"asilxx1");
            return hit.position;

        }
        else
        {
            Debug.Log("asilxx2");
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
}
