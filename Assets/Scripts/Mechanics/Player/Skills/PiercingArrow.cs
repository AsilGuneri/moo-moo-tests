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
    PlayerMertController playerController;
    UnitMovementController unitMovementController;
    PiercingArrow piercingArrowData;
    Vector3 arrowPos;
    Quaternion arrowRot;
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
        playerController = UnitManager.Instance.GetPlayerController();
        unitMovementController = GetComponent<UnitMovementController>();
    }
    public override void OnSkillStart()
    {
        CachePiercingArrowRotation();
        CachePiercingArrowPosition();
        transform.rotation = arrowRot;
        playerController.Animator.CrossFade("PiercingArrow", 0.25f);
        playerController.IsCastingSkill = true;
        unitMovementController.ClientStop();
        
    }
    public override void OnSkillInterrupt()
    {
        //throw new System.NotImplementedException();
    }
    public override void OnSkillEnd()
    {
        GameObject arrow = GetComponent<SkillSpawner>().SpawnSkillPrefab(piercingArrowData.Name);
        SkillProjectile projectile = arrow.GetComponent<SkillProjectile>();
        projectile.SetupProjectile(50, transform);
        arrow.transform.rotation = arrowRot;
        arrow.transform.position = arrowPos;
        playerController.IsCastingSkill = false;
    }
    public override void OnSkillStay()
    {
        //throw new System.NotImplementedException();
    }
    private void CachePiercingArrowRotation()
    {
        // Calculate the direction vector from the start to the end transform
        Vector3 direction = Extensions.GetMouseHitPosition() - transform.position;

        // Calculate the rotation needed to face the direction vector
        Quaternion rotation = Quaternion.LookRotation(direction);
        arrowRot = Quaternion.Euler(new Vector3(0, rotation.eulerAngles.y, rotation.eulerAngles.z));
    }
    private void CachePiercingArrowPosition()
    {
        var p = transform.position + ((Extensions.GetMouseHitPosition() - transform.position).normalized * 1);
        arrowPos =  new Vector3(p.x, 1, p.z);
    }
}