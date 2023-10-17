using DuloGames.UI;
using Mirror;
using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public bool HasIndicator;
    [ConditionalField(nameof(HasIndicator), false)] public GameObject IndicatorPrefab;

    public bool HasCastTime;
    [ConditionalField(nameof(HasCastTime), false)] public float CastTime;
    [ConditionalField(nameof(HasCastTime), false)] public bool BlockMovementOnCast;
    [ConditionalField(nameof(HasCastTime), false)] public bool BlockAttackOnCast;



    public bool HasSkillTime;
    [ConditionalField(nameof(HasSkillTime), false)] public float SkillTime;
    [ConditionalField(nameof(HasSkillTime), false)] public bool BlockMovementOnSkill;
    [ConditionalField(nameof(HasSkillTime), false)] public bool BlockAttackOnSkill;


    public float CooldownTime;

    public UISpellInfo skillInfo;

}
public abstract class SkillController : NetworkBehaviour
{
    public Skill SkillData;
    protected bool isCasting;
    protected bool isSkillActive;
    protected SkillIndicator currentIndicator;
    protected UnitController controller;

    protected virtual void Awake()
    {
        controller = GetComponent<UnitController>();
    }

    public void Use()
    {
        StartCast();
    }

    public void StartIndicator()
    {
        SkillIndicator indicator = PrefabPoolManager.Instance.GetFromPool(SkillData.IndicatorPrefab, Vector3.zero, Quaternion.identity).GetComponent<SkillIndicator>();
        indicator.Setup();
        currentIndicator = indicator;
    }

    public void EndIndicator()
    {
        currentIndicator.EndIndicator();
        currentIndicator = null;
        StartCast();
    }

    private void StartCast()
    {
        isCasting = true;
        if (SkillData.BlockMovementOnCast)
        {
            controller.Movement.BlockMovement();
        }
        if (SkillData.BlockAttackOnCast)
        {
            controller.AttackController.BlockAttacking();
        }
        StartCoroutine(EndCastRoutine());
        OnCastStart();
    }
    private void EndCast()
    {
        isCasting = false;
        if (SkillData.BlockMovementOnCast)
        {
            controller.Movement.RemoveMovementBlock();
        }
        if (SkillData.BlockAttackOnCast)
        {
            controller.AttackController.RemoveAttackingBlock();
        }
        StartSkill();
        OnCastEnd();
    }

    private void StartSkill()
    {
        isSkillActive = false;
        if (SkillData.BlockMovementOnSkill)
        {
            controller.Movement.BlockMovement();
        }
        if (SkillData.BlockAttackOnSkill)
        {
            controller.AttackController.BlockAttacking();
        }
        StartCoroutine(EndSkillRoutine());
        OnSkillStart();

    }
    private void EndSkill()
    {
        isSkillActive = false;
        if (SkillData.BlockMovementOnSkill)
        {
            controller.Movement.RemoveMovementBlock();
        }
        if (SkillData.BlockAttackOnSkill)
        {
            controller.AttackController.RemoveAttackingBlock();
        }
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
            var castTime = SkillData.HasCastTime ? SkillData.CastTime : 0;
            yield return Extensions.GetWait(castTime);
        }
        EndCast();
    }
    private IEnumerator EndSkillRoutine()
    {
        if (isSkillActive)
        {
            var castTime = SkillData.HasSkillTime ? SkillData.SkillTime : 0;
            yield return Extensions.GetWait(castTime);
        }
        EndSkill();
    }
}