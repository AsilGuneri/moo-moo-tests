using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Utilities;

public class TowerManager : NetworkSingleton<TowerManager> 
{
    [SerializeField] private Transform towerParent;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private Transform[] towerPositions;

    [Command(requiresAuthority = false)]
    public void SetTowers()
    {
        foreach(var pos in towerPositions)
        {
            var tower = PrefabPoolManager.Instance.GetFromPool(towerPrefab, pos.position, Quaternion.identity);
            NetworkServer.Spawn(tower);
        }
    }

}
