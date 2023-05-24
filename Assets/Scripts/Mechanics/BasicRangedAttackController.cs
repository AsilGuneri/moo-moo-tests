using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Mirror;
using UnityEngine.UIElements;
using MyBox;
using System.Threading.Tasks;

public class BasicRangedAttackController : ABasicAttackController
{

    [Separator("Range Options")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;


    [Command(requiresAuthority = false)]
    private void CmdSpawnProjectile()
    {
        //if (checkAuthority && !hasAuthority) return;

        GameObject projectile = ObjectPooler.Instance.SpawnFromPoolWithPrefab(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetupProjectile(tc.Target, baseStats.Damage, transform);
        NetworkServer.Spawn(projectile, connectionToClient);
    }
    private async void DelayProjectileSpawn()
    {
        ac.SetAutoAttackStatus(true);
        int msBeforeAttack = Extensions.ToMiliSeconds(((1 / baseStats.AttackSpeed) * baseStats.AnimAttackMoment));
        int msAfterAttack = Extensions.ToMiliSeconds((1 / baseStats.AttackSpeed) * (1 - baseStats.AnimAttackMoment));
        await Task.Delay(msBeforeAttack);
        CmdSpawnProjectile();
        await Task.Delay(msAfterAttack);
        IsAttacking = false;
    }

    protected override void StopAttacking()
    {
        if (IsAttacking)
        {
            IsAttacking = false;
            ac.SetAutoAttackStatus(false);
        }
    }

    protected override void StartAttacking()
    {
        transform.LookAt(new Vector3(tc.Target.transform.position.x, transform.position.y, tc.Target.transform.position.z));
        if (umc) umc.ClientStop();
        IsAttacking = true;
        DelayProjectileSpawn();
        counter = 0;
    }
}
