using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PiercingArrow", menuName = "Scriptable Objects/PiercingArrow", order = 1)]
public class PiercingArrow : SkillData
{
    public GameObject Prefab;
    public override void SetController(GameObject playerObj)
    {
        if(playerObj.TryGetComponent(out SkillController baseSkillController))
        {
            Destroy(baseSkillController);
        }
        if (!playerObj.TryGetComponent(out PiercingArrowController skillController))
        {
            skillController = playerObj.AddComponent<PiercingArrowController>();
            skillController.OnSetup(this);
        }
    }
}
public class PiercingArrowController : SkillController
{
    PiercingArrow piercingArrowData;
    //Override if needed
    public override void UseSkill()
    {
        base.UseSkill();
    }
    public override void OnSetup(SkillData skillData)
    {
        base.OnSetup(skillData);
        piercingArrowData = (PiercingArrow) skillData;
        GetComponent<SkillSpawner>().RegisterPrefab(piercingArrowData.Name, piercingArrowData.Prefab);
    }
    public override void OnSkillStart()
    {
        Debug.Log($"Started Skill : {SkillData.name}");
        GetComponent<SkillSpawner>().SpawnPiercingArrow();
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