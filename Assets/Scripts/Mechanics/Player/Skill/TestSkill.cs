using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Skills/SkillData/Test", fileName = "TestSkill")]

public class TestSkill : SkillData
{
    public override Type GetSkillControllerType()
    {
        return typeof(TestSkillController);
    }
}
public class TestSkillController : SkillController
{
    private TestSkill testSkill;
    public override void SetupSkill(SkillData skillData)
    {
        testSkill = (TestSkill) skillData;
    }
    public override void OnSkillEnd()
    {
        
    }

    public override void OnSkillStart()
    {
        Debug.Log("Skill is started");
    }
}
