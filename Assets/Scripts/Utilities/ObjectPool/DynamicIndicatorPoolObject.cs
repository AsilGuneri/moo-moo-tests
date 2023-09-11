using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseFollowingWorldObject))]
public class DynamicIndicatorObject : PoolObject
{
    MouseFollowingWorldObject mouseFollow;
    private void Awake()
    {
        mouseFollow = GetComponent<MouseFollowingWorldObject>();
    }
    public override void OnSpawn()
    {
        mouseFollow.StartFollowing();
    }
    public override void OnReturn()
    {
        mouseFollow.StopFollowing();
    }
}
