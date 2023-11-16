using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyController : UnitController
{
    public MinionType MinionType { get => minionType; }

    [SerializeField] protected MinionType minionType;


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (!isServer) return;
        SubscribeEvents();
    }

    public override void OnDeath(Transform k)
    {
        base.OnDeath(k);
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
