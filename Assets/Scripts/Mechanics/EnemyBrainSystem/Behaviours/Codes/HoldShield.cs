using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HoldShield", menuName = "Scriptable Objects/EnemyBehaviours/HoldShield")]
public class HoldShield : EnemyBehaviourData
{
    public float Time;
    public float Cooldown;
    public override EnemyBehaviourController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<HoldShieldController>();
        gameObject.GetComponent<EnemyBrain>().StateControllerDictionary.Add(this.name, controller);
        return controller;
    }
}
public class HoldShieldController : EnemyBehaviourController
{
    private HoldShield defendData;
    private UnitController controller;


    public override void OnInitialize(EnemyBehaviourData data)
    {
        base.OnInitialize(data);
        defendData = data as HoldShield;
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