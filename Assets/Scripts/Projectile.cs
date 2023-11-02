using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public interface IProjectile
{
    void SetupProjectile(GameObject target, int damage, Transform spawnerTransform);
    void UpdateProjectile();
    void DestroySelf();
}

public class Projectile : NetworkBehaviour, IProjectile
{
    [SerializeField] private bool is3D = false;
    [SerializeField] private Collider hitCollider;

    [SerializeField] public float speed = 1;
    [SerializeField] private GameObject onHitParticlePrefab;

    [SyncVar] private bool _isMoving;
    [SyncVar] private int _damage;
    [SyncVar] private Transform spawnerTransform;
    [SyncVar] private GameObject Target;

    public bool BelongsToEnemy(UnitType enemyTo)
    {
        return spawnerTransform.GetComponent<UnitController>().IsEnemyTo(enemyTo);
    }
    [Server]
    public void SetupProjectile(GameObject target, int damage, Transform spawnerTransform)
    {
        _isMoving = true;
        Target = target;
        _damage = damage;
        this.spawnerTransform = spawnerTransform;
    }

    public void Update()
    {
        UpdateProjectile();
    }

    public void UpdateProjectile()
    {

        if (_isMoving && Target == null) DestroySelf();
        if (Target == null || !_isMoving) return;

        UnitController targetController = Target.GetComponent<UnitController>();
        bool isCloseEnough = Extensions.CheckRangeBetweenUnitAndCollider(targetController, hitCollider, 0.1f);

        Vector3 targetPos = is3D ? targetController.HitPoint :
            new Vector3(targetController.HitPoint.x, transform.position.y, targetController.HitPoint.z);

        if (!isCloseEnough)
        {
            transform.LookAt(targetPos);
            transform.position += (transform.forward).normalized * Time.deltaTime * speed;
        }
        else
        {
            CmdNotifyHit(Target);
        }
    }

    [Server]
    public void DestroySelf()
    {
        NetworkServer.UnSpawn(gameObject);
        PrefabPoolManager.Instance.PutBackInPool(gameObject);
    }

    [Command(requiresAuthority = false)]
    public void CmdNotifyHit(GameObject target)
    {
        if (!IsValidHit(target))
        {
            return;
        }

        if (onHitParticlePrefab)
        {
            var obj = PrefabPoolManager.Instance.GetFromPool(onHitParticlePrefab, transform.position, transform.rotation);
            NetworkServer.Spawn(obj);
        }

        target.GetComponent<Health>().TakeDamage(_damage, spawnerTransform);
        DestroySelf();
    }

    [Server]
    private bool IsValidHit(GameObject target)
    {
        if(!target) return false;
        // Logic to validate if the hit was legitimate goes here.
        // E.g., check distances, validate that the target was not behind a wall, etc.
        // Returning true for now as a placeholder.
        return true;
    }
}
