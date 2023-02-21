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
    public GameObject SpawnSkillPrefab(string skillName)
    {
        GameObject projectile = Instantiate(SkillNamePrefabPairs[skillName], transform.position, Quaternion.identity);
        NetworkServer.Spawn(projectile, connectionToClient);
        return projectile;
    }
}
