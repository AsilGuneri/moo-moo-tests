using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Utilities;
using UnityEngine.InputSystem.XR;

public class TowerManager : NetworkSingleton<TowerManager> 
{
    [SerializeField] private Transform towerParent;
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private TowerController[] towers;

    //[Command(requiresAuthority = false)]
    //public void SetTowers()
    //{
    //    foreach(var tower in towers)
    //    {
    //    }
    //}
    public void SetMainHall()
    {

    }

}
