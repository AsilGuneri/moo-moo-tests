using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BasicAttackController : NetworkBehaviour
{
    //temp
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    public int Damage;
    Transform currentTarget = null;

    public async void StartAutoAttack(Transform target, float attackSpeed, float animAttackPoint)
    {
        while (IsAttackingAvailable())
        {
            await AttackOnce(target, attackSpeed, animAttackPoint);
        }
    }
    private async Task AttackOnce(Transform target, float attackSpeed, float animAttackPoint)
    {
        Vector3 lookPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(lookPos);
        currentTarget = target;
        await DelayProjectileSpawn(attackSpeed, animAttackPoint);
    }
    private async Task DelayProjectileSpawn(float attackSpeed,float animAttackPoint)
    {
        Extensions.GetAttackTimes(attackSpeed, animAttackPoint
            , out int msBeforeAttack, out int msAfterAttack);

        await Task.Delay(msBeforeAttack);
        CmdSpawnProjectile();
        await Task.Delay(msAfterAttack);
    }
    [Command(requiresAuthority = false)]
    private void CmdSpawnProjectile()
    {
        //if (checkAuthority && !hasAuthority) return;
        GameObject projectile = ObjectPooler.Instance.Get(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetupProjectile(currentTarget.gameObject, Damage, transform);
        NetworkServer.Spawn(projectile, connectionToClient);
    }
    private bool IsAttackingAvailable()
    {
        return true;
    }
}
