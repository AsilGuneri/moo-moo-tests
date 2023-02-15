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
    //Required Mana
    public int ManaCost;
    //Skills cooldown without any buff/boost.
    public float BaseCooldown;

    public virtual void SetController(GameObject playerObj)
    {
        if(!playerObj.TryGetComponent(out SkillController skillController))
        {
            skillController = playerObj.AddComponent<PiercingArrowController>();
            skillController.OnSetup(this);
        }
    }
    public SkillController GetController(GameObject playerObj)
    {
        return playerObj.GetComponent<PiercingArrowController>();
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
