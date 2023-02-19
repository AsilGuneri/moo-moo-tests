using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkillProjectile : NetworkBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] float range;

    [SyncVar] private bool isStarted = false;
    [SyncVar] private int _damage;

    private Vector2 startPoint;

    public void SetupProjectile(int damage, Transform startTransform)
    {
        isStarted = true;
        _damage = damage;
        startPoint = Extensions.Vector3ToVector2(startTransform.position);
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (isStarted) return;
        bool shouldMove = false;
        shouldMove = Vector2.Distance(Extensions.Vector3ToVector2(transform.position), startPoint) <= range;
        if (shouldMove)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            return;
        }
        else
        {
            isStarted = false;
            DestroySelf();
        }
    }
    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }

}
