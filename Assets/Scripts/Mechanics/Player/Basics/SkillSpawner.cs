using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawner : NetworkBehaviour
{
    public Dictionary<string, GameObject> SkillNamePrefabPairs = new Dictionary<string, GameObject>();

    public void RegisterPrefab(string name, GameObject prefab)
    {
        if (SkillNamePrefabPairs.ContainsKey(name)) return;
        SkillNamePrefabPairs.Add(name, prefab);
    }

    public void SpawnPiercingArrow()
    {
        GameObject projectile = Instantiate(SkillNamePrefabPairs["PiercingArrow"], transform.position, Quaternion.identity);
        projectile.GetComponent<SkillProjectile>().SetupProjectile(50, transform);
        NetworkServer.Spawn(projectile, connectionToClient);
    }
}
