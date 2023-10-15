using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SkillCallbackProvider : NetworkSingleton<SkillCallbackProvider>
{
    [Command(requiresAuthority = false)]
    public void SpawnTopDownArrows(GameObject prefab, Vector3 point)
    {
        var skillObj = PrefabPoolManager.Instance.GetFromPool(prefab, point, Quaternion.identity);
        NetworkServer.Spawn(skillObj);
    }
}
