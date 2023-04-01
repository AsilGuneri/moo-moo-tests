using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Pathfinding;
using Pathfinding.RVO;
using UnityEditor.Rendering;

public class UnitMovementController : MonoBehaviour
{


    private TargetController targetController;
    private AnimationController animationController;
    private IAstarAI aiMovement;

    public float rotationSpeed = 5f;

    private void Awake()
    {
        targetController = GetComponent<TargetController>();
        animationController = GetComponent<AnimationController>();
        aiMovement = GetComponent<IAstarAI>();
    }
    private void Update()
    {
        RotateTowardsSteeringTarget();
    }
    private void RotateTowardsSteeringTarget()
    {
        if (aiMovement.steeringTarget != null && aiMovement.velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(aiMovement.steeringTarget - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
    public void ClientMove(Vector3 pos, bool movingToTarget = false, float stoppingDistance = 2f)
    {
        if (animationController != null)
        {
            animationController.OnAttackEnd();
            animationController.OnMove();
        }
        if (!movingToTarget) targetController.SyncTarget(null);
        //dont forget the stopping distance part
        aiMovement.destination = pos;
    }



    public void ClientStop()
    {
        if (animationController != null) animationController.OnStop();
        aiMovement.isStopped = true;
    }

}
