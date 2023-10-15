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
        
    }
}
