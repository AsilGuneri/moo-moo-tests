using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoolObject : PoolObject
{
    protected override void OnSpawn()
    {
        base.OnSpawn();
        GetComponent<TowerController>().StartTower();
    }
    protected override void OnReturn()
    {
        base.OnReturn();
        GetComponent<TowerController>().StopTower();

    }
}
