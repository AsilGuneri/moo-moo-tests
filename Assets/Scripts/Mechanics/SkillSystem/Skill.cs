using Mirror;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string Name;
    public bool HasCastTime;
    [ConditionalField(nameof(HasCastTime), true)] public float CastTime;

    public bool HasSkillTime;
    [ConditionalField(nameof(HasSkillTime), true)] public float SkillTime;

    public float CooldownTime;

    protected bool isOnCooldown = false;

    public abstract SkillController CreateBehaviourController(GameObject gameObject);

    public virtual void Initialize(Transform owner)
    {
        // Create the appropriate controller
        var controller = CreateBehaviourController(owner.gameObject);
        controller.OnInitialize(this);
    }
}
public abstract class SkillController : MonoBehaviour
{
    private Skill skill;
    protected bool isCasting;
    protected bool isSkillActive;
    /// <summary>
    /// Override and keep the base of that method
    /// </summary>
    /// <param name="data"></param>
    public virtual void OnInitialize(Skill skill)
    {
        this.skill = skill;
    }

    protected virtual void StartCast()
    {
        isCasting = true;
        StartCoroutine(EndCastRoutine());
    }
    protected virtual void EndCast()
    {
        isCasting = false;
        StartSkill();
    }

    protected virtual void StartSkill()
    {
        isSkillActive = false;
        StartCoroutine(EndSkillRoutine());

    }
    protected virtual void EndSkill()
    {
        isSkillActive = false;
    }

    private IEnumerator EndCastRoutine()
    {
        if (isCasting)
        {
            var castTime = skill.HasCastTime ? skill.CastTime : 0;
            yield return Extensions.GetWait(castTime);
        }
        EndCast();
    }
    private IEnumerator EndSkillRoutine()
    {
        if (isSkillActive)
        {
            var castTime = skill.HasSkillTime ? skill.SkillTime : 0;
            yield return Extensions.GetWait(castTime);
        }
        EndSkill();
    }
}