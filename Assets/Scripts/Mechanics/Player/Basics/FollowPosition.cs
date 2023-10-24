using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    private Vector3 initialOffset;
    private Transform targetTransform;
    void LateUpdate()
    {
        if (!targetTransform) return;
        transform.position = targetTransform.position + initialOffset;
    }
    public void Setup(Camera cam, Transform target)
    {
        initialOffset = cam.transform.position - target.position;
        targetTransform = target;
    }
}
