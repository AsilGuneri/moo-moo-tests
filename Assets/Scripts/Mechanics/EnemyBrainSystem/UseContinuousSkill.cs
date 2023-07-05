using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UseContinuousSkill", menuName = "ScriptableObjects/EnemyBehaviours/UseContinuousSkill")]

public class UseContinuousSkill : EnemyBehaviourData
{
    public SkillData Skill;
    // Create the UseContinuousSkillController and add it to the given game object
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<UseContinuousSkillController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(State, controller);
        return controller;
    }
}
public class UseContinuousSkillController : EnemyBehaviourController
{
    private UseContinuousSkill data;
    private EnemyController controller;
    private bool shouldEnter = true;

    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        this.data = data as UseContinuousSkill;
        controller = GetComponent<EnemyController>();
    }

    public override bool EnterCondition()
    {
        return shouldEnter;
    }

    public override bool ExitCondition()
    {
        return !shouldEnter;
    }

    public override void OnEnter()
    {
        shouldEnter = false;
        data.Skill.GetController(gameObject).UseSkill();
    }

    public override void OnExit()
    {

    }
}
