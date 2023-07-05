using Mirror;
using ProjectDawn.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ContinuousHeal", menuName = "Scriptable Objects/ContinuousHeal", order = 1)]

public class ContinuousHeal : SkillData
{
    public GameObject HealSourcePrefab;
    public override void SetController(GameObject playerObj)
    {
        if (playerObj.TryGetComponent(out SkillController baseSkillController))
        {
            Destroy(baseSkillController);
        }
        if (!playerObj.TryGetComponent(out ContinuousHealController skillController))
        {
            skillController = playerObj.AddComponent<ContinuousHealController>();
            skillController.OnSetup(this);
        }
    }
}
public class ContinuousHealController : SkillController
{
    UnitController controller;
    ContinuousHeal data;
    public override void UseSkill()
    {
        base.UseSkill();
    }
    public override void OnSetup(SkillData skillData)
    {
        base.OnSetup(skillData);
        data = (ContinuousHeal)skillData;
        controller = GetComponent<UnitController>();
    }
    public override void OnSkillStart()
    {
        GameObject healSource = ObjectPooler.Instance.Get(data.HealSourcePrefab, transform.position + transform.forward * 2 ,Quaternion.identity);
        NetworkServer.Spawn(healSource);
    }
    public override void OnSkillInterrupt()
    {
        //throw new System.NotImplementedException();
    }
    public override void OnSkillEnd()
    {

    }
    public override void OnSkillStay()
    {
        //throw new System.NotImplementedException();
    }
}