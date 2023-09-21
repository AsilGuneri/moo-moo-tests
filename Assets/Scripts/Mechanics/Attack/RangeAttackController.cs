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
        CmdSpawnProjectile(controller.TargetController.Target.gameObject, connectionToClient);
    }
    protected override void OnAttackEnd()
    {

    }
    [Command(requiresAuthority = false)]
    private void CmdSpawnProjectile(GameObject target, NetworkConnectionToClient client)
    {
        //if (checkAuthority && !hasAuthority) return;
        GameObject projectile = PrefabPoolManager.Instance.GetFromPool(controller.ProjectileSpawnPoint.position, Quaternion.identity, projectilePrefab);
            projectile.GetComponent<Projectile>().SetupProjectile(target, Damage, transform);
            NetworkServer.Spawn(projectile, connectionToClient);
    }

}
