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

    public bool HasSkillTime;
    [ConditionalField(nameof(HasSkillTime), false)] public float SkillTime;

    public float CooldownTime;

    public UISpellInfo skillInfo;

}
public abstract class SkillController : NetworkBehaviour
{
    public Skill SkillData;
    protected bool isCasting;
    protected bool isSkillActive;
    protected SkillIndicator currentIndicator;

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