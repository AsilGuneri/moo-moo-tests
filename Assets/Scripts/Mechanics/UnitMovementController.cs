using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Pathfinding;

public class UnitMovementController : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    private TargetController targetController;
    private AnimationController animationController;
 
    private RichAI richAI;

    private void Awake()
    {
        targetController = GetComponent<TargetController>();
        richAI = GetComponent<RichAI>();
        animationController = GetComponent<AnimationController>();
    }

    private void Update()
    {
        if (richAI.reachedEndOfPath || richAI.pathPending)
        {
            return;
        }

        Vector3 targetDirection = (richAI.steeringTarget - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move the character at a constant speed
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, moveSpeed * Time.deltaTime);
    }

    public void ClientMove(Vector3 pos, bool movingToTarget = false, float stoppingDistance = 0.05f)
    {
        if (animationController != null)
        {
            animationController.OnAttackEnd();
            animationController.OnMove();
        }
        if (!movingToTarget) targetController.SyncTarget(null);

        richAI.endReachedDistance = stoppingDistance;
        richAI.destination = pos;
    }

    public void ClientStop()
    {
        if (animationController != null) animationController.OnStop();

        richAI.destination = transform.position;
    }
}
