using Steamworks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class SkillData : ScriptableObject ///TODO: Rename to Skill
{
    //Name will be identifier to find or get skills.
    public string Name;
    //Grade will separate possible q skills from w,e,r etc.
    public int Tier;
    //Skills cooldown without any buff/boost.
    public float BaseCooldown;
    //Required time values of a skill.
    public float CastTime;
    public float CastStartDelay;
    public float OnSkillStayInterval;
    public bool BlockMovement;
    public bool BlockAttacking;

    [Header("Skill Specific")]
    public Sprite Icon;

    public abstract void SetController(GameObject playerObj);

    public SkillController GetController(GameObject playerObj)
    {
        return playerObj.GetComponent<SkillController>();
    }

}
public abstract class SkillController : MonoBehaviour
{
    protected SkillData SkillData;
    protected bool isOnCooldown;
    protected UnitController unitController;
    private bool isSkillStayActive;

    public virtual void OnSetup(SkillData skillData)
    {
        SkillData = skillData;
        unitController = GetComponent<UnitController>();
    }
    public virtual void OnSkillStart()
    {
        if (SkillData.BlockMovement)
        {
            unitController.Movement.BlockMovement();
        }
        if (SkillData.BlockAttacking)
        {
            unitController.AttackController.BlockAttacking();
        }
    }
    public abstract void OnSkillInterrupt();  //Not implemented yet.
    public virtual void OnSkillEnd()
    {
        if (SkillData.BlockMovement)
        {
            unitController.Movement.RemoveMovementBlock();
        }
        if (SkillData.BlockAttacking)
        {
            unitController.AttackController.RemoveAttackingBlock();
        }
    }
    public abstract void OnSkillStay();
    public virtual void UseSkill()
    {
        CastSkill();
    }
    private async void CastSkill()
    {
        if (isOnCooldown)
        {
            return;
        }
        StartCooldown();
        await Task.Delay(Extensions.ToMiliSeconds(SkillData.CastStartDelay));
        OnSkillStart();
        isSkillStayActive = true;
        StartOnSkillStay();
        await Task.Delay(Extensions.ToMiliSeconds(SkillData.CastTime));
        isSkillStayActive = false;
        OnSkillEnd();
    }
    private async Task StartOnSkillStay()
    {
        while (isSkillStayActive)
        {
            OnSkillStay();
            await Task.Delay(Extensions.ToMiliSeconds(SkillData.OnSkillStayInterval));
        }
    }
    private async void StartCooldown()
    {
        isOnCooldown = true;
        float requiredMinTime = SkillData.CastStartDelay + SkillData.CastTime;
        float cooldown = SkillData.BaseCooldown < requiredMinTime ? requiredMinTime : SkillData.BaseCooldown;
        SkillBar.Instance.OnCooldownStart(SkillData.Tier, cooldown);
        await Task.Delay(Extensions.ToMiliSeconds(cooldown));
        isOnCooldown = false;
    }
}
public enum Class
{
    Archer,
    TestClass
}
