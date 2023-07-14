using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UseFocusSkill", menuName = "ScriptableObjects/EnemyBehaviours/UseFocusSkill")]
public class UseFocusSkill : EnemyBehaviourData
{
    public float Time;
    public float Cooldown;
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<UseFocusSkillController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(this.name, controller);
        return controller;
    }
}
public class UseFocusSkillController : EnemyBehaviourController
{
    private UseFocusSkill focusData;
    private UnitController controller;
    private Skill focusSkill;
    private float counter = 0;
    private bool isIn = false;

    private float cooldownCounter = 0;
    private bool isOnCooldown = false;

    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        focusData = data as UseFocusSkill;
        controller = GetComponent<UnitController>();
        focusSkill = GetFirstFocusSkill();
    }

    private void Update()
    {
        if (isOnCooldown)
        {
            cooldownCounter+= Time.deltaTime;
            if (cooldownCounter >= focusData.Cooldown)
            {
                isOnCooldown = false;
            }
        }

        if (isIn)
        {
            counter += Time.deltaTime;
            if (counter >= focusData.Time)
            {
                isIn = false;
            }
        }
    }

    public override bool EnterCondition()
    {
        if (focusSkill == null) return false;
        if(isOnCooldown) return false;
        return true;
    }

    public override bool ExitCondition()
    {
        return !isIn;
    }

    public override void OnEnter()
    {
        focusSkill.Use(controller);
        StartCooldown();
        counter = 0;
        isIn = true;
    }
    public override void OnExit()
    {
        counter = 0;
        isIn = false;
    }
    private Skill GetFirstFocusSkill()
    {
        foreach (var skill in controller.Skills)
        {
            if (skill.IsFocusingSkill)
            {
                return skill;
            }
        }
        Debug.LogError("No focus skill found");
        return null;
    }
    private void StartCooldown()
    {
        cooldownCounter = 0;
        isOnCooldown = true;
    }
    

}