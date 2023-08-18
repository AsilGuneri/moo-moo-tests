using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoolObject : PoolObject
{
    public override void OnSpawn()
    {
        base.OnSpawn();
        GetComponent<TowerController>().StartTower();
    }
    public override void OnReturn()
    {
        base.OnReturn();
        GetComponent<TowerController>().StopTower();

    }
}
