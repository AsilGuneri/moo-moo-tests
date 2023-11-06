using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyController : UnitController
{
    public MinionType MinionType { get => minionType; }

    [SerializeField] protected MinionType minionType;

    void Start()
    {
        SubscribeEvents();
    }

    public override void OnDeath(Transform k)
    {
        UnitManager.Instance.UnregisterUnits(this);
        if (k.TryGetComponent(out PlayerLevelController levelController))
        {
            var unit = k.GetComponent<UnitController>();
            levelController.GainExp(unit.Health.ExpToGain);
        }
        NetworkServer.UnSpawn(gameObject);
        PrefabPoolManager.Instance.PutBackInPool(gameObject);
        //throw new System.NotImplementedException();
    }
}
