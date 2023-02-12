using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PiercingArrow", menuName = "Scriptable Objects/PiercingArrow", order = 1)]
public class PiercingArrow : SkillData
{
    public int a;

}
public class PiercingArrowController : SkillController
{
    private PiercingArrow skillData;

    public override void UseSkill()
    {
        base.UseSkill();
    }
    public override void OnSetup(SkillData skillData)
    {
        this.skillData = (PiercingArrow)skillData;
        Debug.Log(this.skillData.a);

    }
    public override void OnSkillStart()
    {
        Debug.Log($"Started Skill : {skillData.name}");
    }
    public override void OnSkillInterrupt()
    {
        //throw new System.NotImplementedException();
    }
    public override void OnSkillEnd()
    {
        Debug.Log($"Ended Skill : {skillData.name}");
    }
    public override void OnSkillStay()
    {
        //throw new System.NotImplementedException();
    }
}