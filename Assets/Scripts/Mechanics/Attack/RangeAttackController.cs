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
        CmdSpawnProjectile(controller.TargetController.Target);
    }
    protected override void OnAttackEnd()
    {

    }
    [Command(requiresAuthority = false)]
    private void CmdSpawnProjectile(GameObject target)
    {
       
            //if (checkAuthority && !hasAuthority) return;
            GameObject projectile = ObjectPooler.Instance.SpawnFromPool(projectilePrefab, controller.ProjectileSpawnPoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetupProjectile(target, Damage, transform);
            NetworkServer.Spawn(projectile, connectionToClient);

    }

}
