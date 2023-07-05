using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : UnitController
{
    [SerializeField] SkillData mainSkill;
    protected override void Start()
    {
        base.Start();
        SubscribeAnimEvents();
    }
    public void TriggerMainSkill() ///TODO: Remove this method and use the one from UseContinuousSkill
    {
        mainSkill.GetController(gameObject).UseSkill();
    }
}
