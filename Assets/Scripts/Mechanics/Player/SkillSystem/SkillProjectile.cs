using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SkillProjectile : NetworkBehaviour
{
    [Tooltip("Bu değişken ananın amıdır.")]
    [SerializeField] float speed = 1;
    [SerializeField] float range;

    [SyncVar] private bool isStarted = false;
    [SyncVar] private int damage;
    [SyncVar] private Transform spawnerTransform;


    private Vector2 startPoint;

    public void SetupProjectile(int damage, Transform spawnerTransform)
    {
        isStarted = true;
        this.damage = damage;
        startPoint = Extensions.Vector3ToVector2(spawnerTransform.position);
        this.spawnerTransform = spawnerTransform;
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!isStarted) return;
        bool shouldMove = false;
        float distance = Vector2.Distance(Extensions.Vector3ToVector2(transform.position), startPoint);
        shouldMove = distance <= range;
        if (shouldMove)
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            return;
        }

        isStarted = false;
        DestroySelf();
        
    }
    [Server]
    private void DestroySelf()
    {
        NetworkServer.Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Health health) && health.UnitType != UnitType.Player)
        {
            health.TakeDamage(damage, spawnerTransform);
        }
    }

}
