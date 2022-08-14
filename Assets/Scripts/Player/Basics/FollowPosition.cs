using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
  public Transform TargetTransform { get; set; }
    void LateUpdate()
    {
        if (!TargetTransform) return;
        transform.position = TargetTransform.position;
    }
}
