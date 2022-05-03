using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    public Transform target;

    // Update is called once per frame
    void LateUpdate()
    {
       if (!target) return;
       transform.position = target.position + offset;
    }
}
