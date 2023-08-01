using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HoldShieldSkill", menuName = "Scriptable Objects/Skills/HoldShieldSkill")]

public class HoldShieldSkill : Skill
{
    public GameObject ShieldPrefab;
    public override SkillController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<HoldShieldSkillController>();
        return controller;
    }
}
public class HoldShieldSkillController : SkillController
{
    private HoldShieldSkill holdShieldData;
    private UnitController controller;
    private GameObject currentShieldObject;
    public override void OnInitialize(Skill skill)
    {
        base.OnInitialize(skill);
        this.skill = skill;
        holdShieldData = skill as HoldShieldSkill;
        controller = GetComponent<UnitController>();
    }
    protected override void OnCastStart()
    {

    }
    protected override void OnCastEnd()
    {

    }

    protected override void OnSkillStart()
    {
        var prefab = holdShieldData.ShieldPrefab;
        currentShieldObject = ObjectPooler.Instance.SpawnFromPool(holdShieldData.ShieldPrefab, controller.ProjectileSpawnPoint.position, Quaternion.identity);

        
    }

    protected override void OnSkillEnd()
    {

    }
}