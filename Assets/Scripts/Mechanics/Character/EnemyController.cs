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
    public void UseMainSkill()
    {
        mainSkill.GetController(gameObject).UseSkill();
    }
}
