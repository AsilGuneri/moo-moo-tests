using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/SkillData/RapidFire", fileName = "RapidFire")]

public class RapidFire : SkillData
{
    public float increaseAmountPercentage;
    public float continuesTime;

    public override Type GetSkillControllerType()
    {
        return typeof(RapidFireController);
    }
}
public class RapidFireController : SkillController
{
    private RapidFire rapidFire;
    private BasicAttackController bac;

    private float counter = 0;
    private bool isCounterStarted = false;

    public override void SetupSkill(SkillData skillData)
    {
        rapidFire = (RapidFire) skillData;
        bac = GetComponent<BasicAttackController>();

    }
    public override void OnSkillEnd()
    {
        bac.AdditionalAttackSpeed = 0;
        isCounterStarted = false;
        counter = 0;
    }

    public override void OnSkillStart()
    {
        isCounterStarted = true;
        bac.AdditionalAttackSpeed += bac.AttackSpeed * (rapidFire.increaseAmountPercentage / 100);

    }
    private void FixedUpdate()
    {
        if (!isCounterStarted) return;
        counter += Time.fixedDeltaTime;
        if (rapidFire.continuesTime < counter) OnSkillEnd();
    }
}
