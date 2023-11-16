using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAttackController : BasicAttackController
{
    [SerializeField] protected GameObject projectilePrefab;
    protected override void OnAttackStart()
    {

    }
    protected override void OnAttackImpact()
    {
        base.OnAttackImpact();
        CmdSpawnProjectile(controller.TargetController.Target.GetComponent<Health>(), connectionToClient);
    }
    protected override void OnEachAttackEnd()
    {

    }
    [Command(requiresAuthority = false)]
    private void CmdSpawnProjectile(Health target, NetworkConnectionToClient client)
    {
        //if (checkAuthority && !hasAuthority) return;
        GameObject projectile = PrefabPoolManager.Instance.GetFromPool(projectilePrefab, controller.ProjectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetupProjectile(target, GetActualDamage(), transform);
        NetworkServer.Spawn(projectile, connectionToClient);
    }

}
