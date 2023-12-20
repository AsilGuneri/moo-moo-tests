using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyController : UnitController
{


    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        if (!isServer) return;
        SubscribeEvents();
    }
    private void Update()
    {
        if (!isServer) return;
        if (!targetController.HasTarget()) SelectTarget();
        else if (!Extensions.CheckRangeBetweenUnits(transform, targetController.Target.transform
            , statController.AttackRange))
        {
            attackController.StopAfterCurrentAttack();
            Movement.ClientMove(targetController.Target.transform.position); 
        }
        else
        {
            Movement.ClientStop();
            attackController.StartAutoAttack();
        }
        
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

    #region Behaviour

    void SelectTarget()
    {
        GameObject priorEnemy = null;

        for (int i = 0; i < enemyList.Count; i++)
        {
            priorEnemy = UnitManager.Instance.GetClosestUnit(transform.position, enemyList[i]);
            if (priorEnemy != null) break;
        }
        targetController.SetTarget(priorEnemy.GetComponent<NetworkIdentity>());
    }

    #endregion

}
