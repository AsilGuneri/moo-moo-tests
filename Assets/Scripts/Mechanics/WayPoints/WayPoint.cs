using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        // Change the Gizmo color to whatever you set
        Gizmos.color = Color.green;

        // Draw a sphere at your object's location
        Gizmos.DrawSphere(transform.position, 0.25f);
    }
}

