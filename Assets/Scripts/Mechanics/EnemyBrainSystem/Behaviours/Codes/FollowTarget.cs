using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowTarget", menuName = "ScriptableObjects/EnemyBehaviours/FollowTarget")]

public class FollowTarget : EnemyBehaviourData
{
    public bool UseAttackRange;
    [ConditionalField(nameof(UseAttackRange),true)] public float FollowOffset;

    // Create the MoveController and add it to the given game object
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<FollowTargetController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(this.name, controller);
        return controller;
    }

}
public class FollowTargetController : EnemyBehaviourController
{
    private FollowTarget followData;
    private bool isIn;
    private UnitController controller;
    private float followOffset;
    private UnitController target;

    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        followData = data as FollowTarget;
        controller = GetComponent<UnitController>();
        followOffset = followData.UseAttackRange ? controller.attackRange: followData.FollowOffset;
    }

    private void Update()
    {
        if (!isIn) return;
    }

    public override bool EnterCondition()
    {
        return ShouldEnter();
    }

    public override bool ExitCondition()
    {
        return !ShouldEnter();
    }

    public override void OnEnter()
    {
        target = controller.TargetController.Target.GetComponent<UnitController>();
        isIn = true;
        controller.Movement.StartFollow(controller.TargetController.Target.transform, followOffset);

    }

    public override void OnExit()
    {
        isIn = false;
        controller.Movement.StopFollow();
    }

    private bool ShouldEnter()
    {
        if (controller.TargetController.Target == null) return false;
        if (Extensions.CheckRange(controller.TargetController.Target.transform.position, transform.position, followOffset)) return false;
        return true;
    }
}