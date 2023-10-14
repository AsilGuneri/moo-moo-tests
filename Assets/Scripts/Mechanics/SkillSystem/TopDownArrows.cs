using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "TopDownArrows", menuName = "Scriptable Objects/Skills/TopDownArrows")]

public class TopDownArrows : Skill
{
    public GameObject ArrowsPrefab;
    public override SkillController CreateBehaviourController(GameObject gameObject)
    {
        var controller = gameObject.AddComponent<TopDownArrowsSkillController>();
        return controller;
    }
}
public class TopDownArrowsSkillController : SkillController
{
    private TopDownArrows arrowsData;
    private PlayerController controller;
    private Vector3 castStartPoint;
    public override void OnInitialize(Skill skill)
    {
        base.OnInitialize(skill);
        this.skill = skill;
        arrowsData = skill as TopDownArrows;
        controller = GetComponent<PlayerController>();
    }
    protected override void OnCastStart()
    {
        Debug.Log($"cast start topdownarrows");
        Ray ray;
        RaycastHit[] hits;
        controller.GetMousePositionRaycastInfo(out ray, out hits);
        RaycastHit? groundHit = hits.FirstOrDefault(hit => hit.collider.gameObject.layer == 6);
        if (groundHit.HasValue)
        {
            castStartPoint = groundHit.Value.point;
        }
    }
    protected override void OnCastEnd()
    {
        Debug.Log($"cast end topdownarrows");
    }

    protected override void OnSkillStart()
    {
        Debug.Log($"skill start topdownarrows");

        var skillObj = PrefabPoolManager.Instance.GetFromPool(arrowsData.ArrowsPrefab, castStartPoint, Quaternion.identity);
            //NetworkServer.
    }

    protected override void OnSkillEnd()
    {
        Debug.Log($"skill end topdownarrows");
    }
}