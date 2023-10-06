using Mirror;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public bool HasCastTime;
    [ConditionalField(nameof(HasCastTime), false)] public float CastTime;

    public bool HasSkillTime;
    [ConditionalField(nameof(HasSkillTime), false)] public float SkillTime;

    public float CooldownTime;

    protected bool isOnCooldown = false;

    public abstract SkillController CreateBehaviourController(GameObject gameObject);

    public virtual void Initialize(Transform owner)
    {
        // Create the appropriate controller
        var controller = CreateBehaviourController(owner.gameObject);
        owner.GetComponent<UnitController>().SkillControllerDictionary.Add(this.name, controller);
        controller.OnInitialize(this);
    }
}
public abstract class SkillController : MonoBehaviour
{
    protected Skill skill;
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

    public void Use()
    {
        StartCast();
    }

    private void StartCast()
    {
        isCasting = true;
        StartCoroutine(EndCastRoutine());
        OnCastStart();
    }
    private void EndCast()
    {
        isCasting = false;
        StartSkill();
        OnCastEnd();
    }

    private void StartSkill()
    {
        isSkillActive = false;
        StartCoroutine(EndSkillRoutine());
        OnSkillStart();

    }
    private void EndSkill()
    {
        isSkillActive = false;
        OnSkillEnd();
    }

    protected abstract void OnCastStart();

    protected abstract void OnCastEnd();

    protected abstract void OnSkillStart();

    protected abstract void OnSkillEnd();

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