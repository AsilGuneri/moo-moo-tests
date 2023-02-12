using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillData : ScriptableObject
{
    //Name will be identifier to find or get skills.
    public string Name;
    //Class will be used to sort skills 
    public Class Class;
    //Grade will separate possible q skills from w,e,r etc.
    public int Grade;

    protected SkillController SkillController;
    public virtual SkillController SetController(GameObject playerObj)
    {
        if(playerObj.TryGetComponent(out SkillController skillController))
        {
            return skillController;
        }
        else
        {
            skillController = playerObj.AddComponent<PiercingArrowController>();
            skillController.OnSetup(this);
            return skillController;
        }
        
    }

}
public abstract class SkillController : MonoBehaviour
{
    public abstract void OnSetup(SkillData skillData);
    public abstract void OnSkillStart();
    public abstract void OnSkillInterrupt();
    public abstract void OnSkillEnd();
    public abstract void OnSkillStay();
    public virtual void UseSkill()
    {
        OnSkillStart();
        OnSkillEnd();
    }
}
public enum Class
{
    Archer
}
