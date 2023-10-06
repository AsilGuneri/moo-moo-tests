using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TopDownArrows", menuName = "Scriptable Objects/Skills/TopDownArrows")]

public class TopDownArrows : Skill
{
    public GameObject ShieldPrefab;
    public override SkillController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<TopDownArrowsSkillController>();
        return controller;
    }
}
public class TopDownArrowsSkillController : SkillController
{
    private TopDownArrows arrowsData;
    private UnitController controller;
    public override void OnInitialize(Skill skill)
    {
        base.OnInitialize(skill);
        this.skill = skill;
        arrowsData = skill as TopDownArrows;
        controller = GetComponent<UnitController>();
    }
    protected override void OnCastStart()
    {
        Debug.Log($"cast start topdownarrows");
    }
    protected override void OnCastEnd()
    {
        Debug.Log($"cast end topdownarrows");
    }

    protected override void OnSkillStart()
    {
        Debug.Log($"skill start topdownarrows");
    }

    protected override void OnSkillEnd()
    {
        Debug.Log($"skill end topdownarrows");
    }
}