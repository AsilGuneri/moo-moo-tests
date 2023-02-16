using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PiercingArrow", menuName = "Scriptable Objects/PiercingArrow", order = 1)]
public class PiercingArrow : SkillData
{

}
public class PiercingArrowController : SkillController
{
    //Override if needed
    public override void UseSkill()
    {
        base.UseSkill();
    }
    public override void OnSetup(SkillData skillData)
    {
        base.OnSetup(skillData);
    }
    public override void OnSkillStart()
    {
        Debug.Log($"Started Skill : {SkillData.name}");
    }
    public override void OnSkillInterrupt()
    {
        //throw new System.NotImplementedException();
    }
    public override void OnSkillEnd()
    {
        Debug.Log($"Ended Skill : {SkillData.name}");
    }
    public override void OnSkillStay()
    {
        //throw new System.NotImplementedException();
    }
}