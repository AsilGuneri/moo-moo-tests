using DG.Tweening;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcherFlashController : SkillController
{
    private ArcherFlash arrowsData;
    private PlayerController playerController;

    protected override void Awake()
    {
        base.Awake();
        arrowsData = SkillData as ArcherFlash;
        playerController = controller as PlayerController;
    }
    protected override void OnCastStart()
    {
        
    }
    protected override void OnCastEnd()
    {
    }

    protected override void OnSkillStart()
    {
        
        var hitPos = Extensions.GetMouseHitPosition();
        bool inRange = Extensions.CheckRange(playerController.transform.position, hitPos, arrowsData.Range);
        Vector3 targetPos;
        if (inRange)
        {
            targetPos = hitPos;
        }
        else
        {
            var playerPos = Extensions.Vector3NoY(transform.position);
            var targetDir = (hitPos - playerPos).normalized;
            targetPos = playerPos + (targetDir * arrowsData.Range);
        }
        Debug.DrawLine(playerController.transform.position, targetPos, Color.red, 5f);
        Debug.DrawLine(playerController.transform.position, hitPos, Color.blue, 5f);

        Vector3 lookPos = new Vector3(targetPos.x, transform.position.y, targetPos.z);
        transform.LookAt(lookPos);
        playerController.AnimationController.StartJump();
        StartCoroutine(JumpRoutine(targetPos, arrowsData.SkillTime));
        playerController.transform.position = targetPos;
    }
    protected override void OnSkillEnd()
    {
        playerController.AnimationController.EndJump();
    }
    private IEnumerator JumpRoutine(Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the transform reaches the target position.
        transform.position = targetPosition;
    }
}
