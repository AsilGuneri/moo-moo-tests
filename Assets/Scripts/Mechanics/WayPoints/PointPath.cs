using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointPath : MonoBehaviour
{
    public WayPoint[] Points;
    private Transform pathCenterPoint;

    private void Start()
    {
        PointPathsManager.Instance.AllPaths.Add(this);
    }

    public Transform PathCenterPoint()
    {
        if (pathCenterPoint != null) return pathCenterPoint;
        else
        {
            int middleIndex = Mathf.CeilToInt(Points.Length / 2);
            pathCenterPoint = Points[middleIndex].transform;
            return pathCenterPoint;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw lines between points
        if (Points != null && Points.Length > 1)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < Points.Length - 1; i++)
            {
                if (Points[i] != null && Points[i + 1] != null)
                {
                    Gizmos.DrawLine(Points[i].transform.position, Points[i + 1].transform.position);
                }
            }
        }
    }
}
