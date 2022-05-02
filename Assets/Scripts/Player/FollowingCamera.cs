using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Transform target;
    [SerializeField] private NetworkIdentity netId;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!netId.hasAuthority) return;
        transform.position = target.position + offset;
    }
}
