using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PointPathsManager : Singleton<PointPathsManager>
{
    public List<PointPath> AllPaths = new List<PointPath>();

    public WayPoint GetClosestPoint(Vector3 position)
    {
        // Ensure there is at least one path and that the first path has at least one waypoint
        if (AllPaths.Count == 0 || AllPaths[0].Points.Length == 0)
        {
            Debug.LogError("No paths or waypoints found.");
            return null;
        }

        // First find the closest path by its center point
        PointPath closestPath = AllPaths[0];
        float closestPathDistance = Vector3.Distance(position, closestPath.PathCenterPoint().position);

        foreach (PointPath path in AllPaths)
        {
            float distance = Vector3.Distance(position, path.PathCenterPoint().position);

            if (distance < closestPathDistance)
            {
                closestPath = path;
                closestPathDistance = distance;
            }
        }

        // Now find the closest waypoint in the closest path
        WayPoint closestPoint = closestPath.Points[0];
        float closestPointDistance = Vector3.Distance(position, closestPoint.transform.position);

        foreach (WayPoint point in closestPath.Points)
        {
            float distance = Vector3.Distance(position, point.transform.position);

            if (distance < closestPointDistance)
            {
                closestPoint = point;
                closestPointDistance = distance;
            }
        }

        return closestPoint;
    }

}
