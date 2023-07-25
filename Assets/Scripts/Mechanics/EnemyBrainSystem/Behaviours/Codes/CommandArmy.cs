using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommandArmy", menuName = "Scriptable Objects/EnemyBehaviours/CommandArmy")]
public class CommandArmy : EnemyBehaviourData
{

    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<CommandArmyController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(this.name, controller);
        return controller;
    }
}
public class CommandArmyController : EnemyBehaviourController
{
    private CommandArmy commandData;
    private UnitController controller;


    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        commandData = data as CommandArmy;
        controller = GetComponent<UnitController>();
    }

    private void Update()
    {

    }

    public override bool EnterCondition()
    {
        return true;
    }

    public override bool ExitCondition()
    {
        return false;
    }

    public override void OnEnter()
    {

    }
    public override void OnExit()
    {

    }

}