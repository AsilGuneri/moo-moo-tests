using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SikisenArrow", menuName = "Scriptable Objects/SikisenArrow", order = 1)]
public class SikisenArrow : SkillData
{
    public override void SetController(GameObject playerObj)
    {
        if (playerObj.TryGetComponent(out SkillController baseSkillController))
        {
            Destroy(baseSkillController);
        }
        if (!playerObj.TryGetComponent(out SikisenArrowController skillController))
        {
            skillController = playerObj.AddComponent<SikisenArrowController>();
            skillController.OnSetup(this);
        }
    }
}
public class SikisenArrowController : SkillController
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
       // Debug.Log($"Ended Skill : {SkillData.name}");
    }
    public override void OnSkillStay()
    {
        //throw new System.NotImplementedException();
    }
}