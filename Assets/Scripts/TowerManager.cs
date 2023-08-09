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

    public void SetTowers()
    {
        foreach(var pos in towerPositions)
        {
            ObjectPooler.Instance.CmdSpawnFromPool(towerPrefab.name, pos.position, Quaternion.identity);
        }
    }

}
