using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public interface IProjectile
{
    // Initialize the projectile.
    void SetupProjectile(GameObject target, int damage, Transform spawnerTransform, Action action = null);

    // Update the projectile.
    void UpdateProjectile();

    // Destroy the projectile.
    void DestroySelf();
}

public class Projectile : NetworkBehaviour, IProjectile
{
    public Action OnHit;

    [SerializeField] public float speed = 1;
    [SerializeField] private GameObject onHitParticlePrefab;

    [SyncVar] private bool _isMoving;
    [SyncVar] private int _damage;
    [SyncVar] private Transform spawnerTransform;
    [NonSerialized][SyncVar] public GameObject Target;


    public bool BelongsToEnemy(UnitType enemyTo)
    {
        return spawnerTransform.GetComponent<UnitController>().IsEnemyTo(enemyTo);
    }

    [Server]
    public void SetupProjectile(GameObject target, int damage, Transform spawnerTransform, Action action = null)
    {
        OnHit = action;
        _isMoving = true;
        Target = target;
        _damage = damage;
        this.spawnerTransform = spawnerTransform;
    }

    [ClientCallback]
    public void Update()
    {
        UpdateProjectile();
    }

    [ClientCallback]
    public void UpdateProjectile()
    {
        if (_isMoving && Target == null) DestroySelf();
        if (Target == null || !_isMoving) return;
        if (Vector2.Distance(Extensions.To2D(transform.position), Extensions.To2D(Target.transform.position)) > 0.4f)
        {
            transform.position += Extensions.Vector3WithoutY(Direction(Target.transform.position) * Time.deltaTime * speed);
            Vector3 targetPos = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            transform.LookAt(targetPos);
            return;
        }
        else
        {
            OnHit?.Invoke();
            CmdTargetHit();
            return;
        }
    }

    private Vector3 Direction(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        return direction;
    }

    public void DestroySelf()
    {
        ObjectPooler.Instance.CmdReturnToPool(gameObject.GetComponent<NetworkIdentity>().netId);
    }

    [ServerCallback]
    private void CmdTargetHit()
    {
        if (Target == null || spawnerTransform == null)
        {
            return;
        }

        Target.GetComponent<Health>().TakeDamage(_damage, spawnerTransform);

        if (onHitParticlePrefab)
        {
            ObjectPooler.Instance.CmdSpawnFromPool(onHitParticlePrefab.name, transform.position, transform.rotation);
        }

        DestroySelf();
    }
}
