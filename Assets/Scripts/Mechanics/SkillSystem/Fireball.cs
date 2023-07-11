using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Fireball : Skill
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int damage;

    public override void Use(UnitController user, UnitController target)
    {
        if (!isOnCooldown && target != null)
        {
            CmdSpawnProjectile(user, target);
            StartCoroutine(Cooldown());
        }
    }
    [Command(requiresAuthority =false)]
    private void CmdSpawnProjectile(UnitController user, UnitController target)
    {
        var spawnPoint = user.ProjectileSpawnPoint;
        var projectileObj = ObjectPooler.Instance.SpawnFromPool(prefab, spawnPoint.position, Quaternion.identity);
        var projectile = projectileObj.GetComponent<Projectile>();
        projectile.SetupProjectile(target.gameObject, damage, user.transform
            , () => { Apply(user, target); });
        NetworkServer.Spawn(projectileObj);

    }
    private void Apply(UnitController user, UnitController target)
    {
        Effect.Apply(user, target);
    }
}

