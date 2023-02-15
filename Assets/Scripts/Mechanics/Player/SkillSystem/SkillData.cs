using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
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
    //How long does it take to cast the skill.
    public float CastTime;
    public float CastStartDelay;
    public float OnSkillStayInterval;

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
    protected SkillData SkillData;
    private bool isSkillStayActive;

    public virtual void OnSetup(SkillData skillData)
    {
        SkillData = skillData;
    }
    public abstract void OnSkillStart();
    public abstract void OnSkillInterrupt();
    public abstract void OnSkillEnd();
    public abstract void OnSkillStay();  //Not implemented yet.
    public virtual void UseSkill()
    {
        CastSkill();
    }
    private async void CastSkill()
    {
        await Task.Delay(Extensions.ToMiliSeconds(SkillData.CastStartDelay));
        OnSkillStart();
        isSkillStayActive = true;
        StartOnSkillStay();
        await Task.Delay(Extensions.ToMiliSeconds(SkillData.CastTime));
        isSkillStayActive = false;
        OnSkillEnd();
    }
    private async void StartOnSkillStay()
    {
        while (isSkillStayActive)
        {
            OnSkillStay();
            await Task.Delay(Extensions.ToMiliSeconds(SkillData.OnSkillStayInterval));
        }
    }
}
public enum Class
{
    Archer
}
