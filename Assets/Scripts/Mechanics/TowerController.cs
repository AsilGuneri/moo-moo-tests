using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TowerController : NetworkBehaviour
{
    [SerializeField] private HeroBaseStatsData towerStats;
    protected Health _hc;
    protected GameObject _target;
    protected AnimationController _ac;
    private TargetController _tc;
    private void Awake()
    {
        _tc = GetComponent<TargetController>();
        _ac = GetComponent<AnimationController>();
    }
    private void FixedUpdate()
    {
        if (_tc.Target == null) PickTarget();
        else if (!UnitManager.Instance.IsInRange(transform, _tc.Target.transform, towerStats.Range))
        {
            _tc.SyncTarget(null);
        }
    }
    protected void PickTarget()
    {
        if (!_tc.Target)
        {
            _tc.Target = UnitManager.Instance.GetUnitInRange(transform.position, towerStats.Range, true);
        }
    }
}