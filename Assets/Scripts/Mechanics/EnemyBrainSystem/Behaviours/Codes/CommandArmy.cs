using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CommandArmy", menuName = "Scriptable Objects/EnemyBehaviours/CommandArmy")]
public class CommandArmy : EnemyBehaviourData
{
    public string CommandPackName;
    public MinionType TargetMinionType;
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
    private EnemyController controller;
    private bool hasEntered;


    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        commandData = data as CommandArmy;
        controller = GetComponent<EnemyController>();
    }

    private void Update()
    {
       
    }

    public override bool EnterCondition()
    {
        return ShouldEnter();//&& !hasEntered;
    }

    public override bool ExitCondition()
    {
        return !ShouldEnter();
    }

    public override void OnEnter()
    {
        hasEntered = true;
        UnitManager.Instance.GiveCommand(commandData.CommandPackName, commandData.TargetMinionType);
    }
    public override void OnExit()
    {

    }
    private bool ShouldEnter()
    {
        if (hasEntered) return false;
        switch (commandData.CommandPackName)
        {
            case "Defend":
                return controller.DefendCommandCondition();
        }
        return false;
    }
}