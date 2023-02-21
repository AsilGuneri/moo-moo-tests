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
        GameObject arrow = GetComponent<SkillSpawner>().SpawnSkillPrefab(piercingArrowData.Name);
        SkillProjectile projectile = arrow.GetComponent<SkillProjectile>();
        projectile.SetupProjectile(50, transform);

        SetPiercingArrowRotation(arrow.transform);
        SetPiercingArrowPosition(arrow.transform);

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
    private void SetPiercingArrowRotation(Transform arrowTransform)
    {
        // Calculate the direction vector from the start to the end transform
        Vector3 direction = Extensions.GetMouseHitPosition() - transform.position;

        // Calculate the rotation needed to face the direction vector
        Quaternion rotation = Quaternion.LookRotation(direction);
        arrowTransform.rotation = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z));
    }
    private void SetPiercingArrowPosition(Transform arrowTransform)
    {
        var p = transform.position + ((Extensions.GetMouseHitPosition() - transform.position).normalized * 1);
        arrowTransform.position = new Vector3(p.x, 1, p.z);
    }
}