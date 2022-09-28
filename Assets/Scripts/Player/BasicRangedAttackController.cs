using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using Mirror;
using UnityEngine.UIElements;
using MyBox;

public class BasicRangedAttackController : ABasicAttackController
{

    [Separator("Range Options")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;


    [Command(requiresAuthority = false)]
    private void CmdSpawnProjectile()
    {
        if (checkAuthority && !hasAuthority) return;
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().SetupProjectile(tc.Target, stats.Damage);
        NetworkServer.Spawn(projectile, connectionToClient);
    }
    private IEnumerator DelayProjectileSpawn()
    {
        yield return new WaitForSeconds((1 / stats.AttackSpeed) / 2);
        CmdSpawnProjectile();
        yield return new WaitForSeconds((1 / stats.AttackSpeed) / 2);
        isAttacking = false;

    }

    protected override void StopAttacking()
    {
        if (isAttacking)
        {
            isAttacking = false;
            StopCoroutine(nameof(DelayProjectileSpawn));
            if (pac) pac.OnAttackEnd();
        }
    }

    protected override void StartAttacking()
    {
        transform.LookAt(new Vector3(tc.Target.transform.position.x, transform.position.y, tc.Target.transform.position.z));
        if (umc) umc.ClientStop();
        if (pac) pac.OnAttackStart(stats.AttackSpeed);
        isAttacking = true;
        StartCoroutine(nameof(DelayProjectileSpawn));
        counter = 0;
    }
}
