using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HoldShieldSkill", menuName = "Scriptable Objects/Skills/HoldShieldSkill")]

public class HoldShieldSkill : Skill
{
    public override SkillController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<HoldShieldSkillController>();
        gameObject.GetComponent<UnitController>().SkillControllerDictionary.Add(this.name, controller);
        return controller;
    }
}
public class HoldShieldSkillController : SkillController
{ 

}