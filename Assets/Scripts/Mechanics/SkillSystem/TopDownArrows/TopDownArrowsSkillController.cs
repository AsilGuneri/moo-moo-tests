using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class TopDownArrowsSkillController : SkillController
{
    private TopDownArrows arrowsData;
    private PlayerController playerController;
    private Vector3 castStartPoint;
    protected override void Awake()
    {
        base.Awake();
        arrowsData = SkillData as TopDownArrows;
        playerController = controller as PlayerController;
    }
    protected override void OnCastStart()
    {
        Debug.Log($"cast start topdownarrows");
        Ray ray;
        RaycastHit[] hits;
        playerController.GetMousePositionRaycastInfo(out ray, out hits);
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
        Debug.Log($"skill start topdownarrows ");
        SpawnArrows(castStartPoint);

    }
    protected override void OnSkillEnd()
    {
        Debug.Log($"skill end topdownarrows");
    }
    [Command(requiresAuthority = false)]
    private void SpawnArrows(Vector3 castStartPos)
    {
        var skillObj = PrefabPoolManager.Instance.GetFromPool(arrowsData.ArrowsPrefab, castStartPos, Quaternion.identity);
        skillObj.GetComponent<TopDownArrowsObject>().Setup(transform);
        NetworkServer.Spawn(skillObj);
    }
}
