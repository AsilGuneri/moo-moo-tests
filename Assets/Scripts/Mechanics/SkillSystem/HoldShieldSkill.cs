using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HoldShieldSkill", menuName = "Scriptable Objects/Skills/HoldShieldSkill")]

public class HoldShieldSkill : Skill
{
    public GameObject ShieldPrefab;

}
public class HoldShieldSkillController : SkillController
{
    private HoldShieldSkill holdShieldData;
    private UnitController controller;
    private GameObject currentShieldObject;

    private void Awake()
    {
        holdShieldData = SkillData as HoldShieldSkill;
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
        currentShieldObject = PrefabPoolManager.Instance.GetFromPool(holdShieldData.ShieldPrefab, controller.ProjectileSpawnPoint.position, Quaternion.identity);
        Transform protectedUnit = UnitManager.Instance.GetClosestUnit(transform.position, UnitType.Player).transform;
        currentShieldObject.GetComponent<ShieldController>().SetupShield(protectedUnit,controller.transform);

        
    }

    protected override void OnSkillEnd()
    {

    }
}