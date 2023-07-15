using MyBox;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToPosition", menuName = "ScriptableObjects/EnemyBehaviours/MoveToPosition")]


public class MoveToPosition : EnemyBehaviourData
{
    public float Offset;
   // Create the MoveController and add it to the given game object
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<MoveToPositionController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(this.name, controller);
        return controller;
    }

}
public class MoveToPositionController : EnemyBehaviourController
{
    private MoveToPosition moveData;
    private UnitController controller;
    private Vector3 targetPos;
    private EnemyBrain brain;
    private FormationPoint currentPoint;

    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        moveData = data as MoveToPosition;
        controller = GetComponent<UnitController>();
        brain = GetComponent<EnemyBrain>();
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
        if (!FormationManager.Instance.IsFormationAvailable("basic"))
        {
            brain.SetPackRoutine("Default");
            return;
        }

        //find your position in formation and move to it
        if(targetPos == Vector3.zero)
        {
            var availablePoint = FormationManager.Instance.UseAvailablePoint("basic", controller.transform);
            targetPos = availablePoint.position;
            currentPoint = availablePoint;
        }
        controller.Movement.SetDestinationOnAvailable(targetPos, true);

    }
    public override void OnExit()
    {
        FormationManager.Instance.LeavePoint(currentPoint);
    }



    private bool ShouldEnter()
    {

        if (Extensions.CheckRange(transform.position,targetPos,moveData.Offset)) return false;
        return true;
    }
}