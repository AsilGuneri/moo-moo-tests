using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public interface IProjectile
{
    // Initialize the projectile.
    void SetupProjectile(GameObject target, int damage, Transform spawnerTransform);

    // Update the projectile.
    void UpdateProjectile();

    // Destroy the projectile.
    void DestroySelf();
}

public class Projectile : NetworkBehaviour, IProjectile
{
    [SerializeField] private bool is3D = false;
    [SerializeField] private Collider hitCollider;


    [SerializeField] public float speed = 1;
    //[SerializeField] public GameObject visualsParent;
    [SerializeField] private GameObject onHitParticlePrefab;

    private bool _isMoving;
    private int _damage;
    private Transform spawnerTransform;
    private GameObject Target;


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

    [Server]
    public void Update()
    {
        UpdateProjectile();
    }

    [Server]
    public void UpdateProjectile()
    {
        if (_isMoving && Target == null) DestroySelf();
        if (Target == null || !_isMoving) return;

        UnitController targetController = Target.GetComponent<UnitController>();
        bool isCloseEnough = Extensions.CheckRangeBetweenUnitAndCollider(targetController, hitCollider, 0.1f);
        // Target position remains for guidance purposes.


        Vector3 targetPos = is3D ? targetController.HitPoint :
            new Vector3(targetController.HitPoint.x, transform.position.y, targetController.HitPoint.z);

        if (!isCloseEnough)
        {
            transform.LookAt(targetPos);
            transform.position += (transform.forward).normalized * Time.deltaTime * speed;
            return;
        }
        else
        {
            TargetHitServer();
            return;
        }
    }


    [Server]
    public void DestroySelf()
    {
        PrefabPoolManager.Instance.ReturnToPoolServer(gameObject);
    }

    [Server]
    private void TargetHitServer()
    {
        if (Target == null || spawnerTransform == null)
        {
            return;
        }
        if (onHitParticlePrefab)
        {
            PrefabPoolManager.Instance.SpawnFromPoolServer(onHitParticlePrefab, transform.position, transform.rotation);
        }
        Target.GetComponent<Health>().TakeDamage(_damage, spawnerTransform);
        DestroySelf();
    }
}
