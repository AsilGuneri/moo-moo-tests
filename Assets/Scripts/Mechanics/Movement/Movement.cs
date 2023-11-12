using UnityEngine;
using System;
using ProjectDawn.Navigation.Hybrid;
using System.Threading.Tasks;
using System.Collections;

public class Movement : MonoBehaviour
{
    public Action OnMoveStart;
    public Action OnMoveStop;

    public float AgentRadius;
    public bool IsFollowing { get => isFollowing; }
    public bool IsMoving { get { return isMoving; } }

    private UnitController controller;
    private bool isMoving = false;
    private Vector3 currentTargetPos;
    private int movementBlockCount = 0;
    private bool isFollowing = false;
    AgentAuthoring agent;
    private Coroutine followCoroutine;

    public Transform Target;



    private void Awake()
    {
        agent = transform.GetComponent<AgentAuthoring>();
        controller = GetComponent<UnitController>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) ClientStop();
        if (!isMoving) return;
        if(controller.Health.IsDead)
        {
            ClientStop();
            return;
        }
        if (ReachedDestination(0.1f) && !controller.TargetController.HasTarget())
        {
            ClientStop();
        }
    }
    public void BlockMovement()
    {
        ClientStop();
        movementBlockCount++;
    }
    public void RemoveMovementBlock()
    {
        movementBlockCount--;
    }
    
    public void ClientMove(Vector3 pos, bool cancelTarget = false)
    {
        if (!CanMove()) return;
        if (cancelTarget) controller.TargetController.SetTarget(null);
        agent.SetDestination(pos);
        isMoving = true;
        currentTargetPos = pos;
        OnMoveStart?.Invoke();
    }

    public void ClientStop()
    {
        agent.Stop();
        currentTargetPos = Vector3.zero;
        isMoving = false;
        OnMoveStop?.Invoke();
    }
    private bool ReachedDestination(float threshold)
    {
        Vector3 destination = currentTargetPos;
        if (destination != Vector3.zero)
        {
            bool x = Extensions.CheckRange(destination, transform.position, threshold);
            return x;
        }
        return false;
    }
    public void StartFollow(Transform target, float followDistance)
    {
        // Stop previous follow coroutine if it exists
        if (isFollowing || followCoroutine != null)
        {
            StopFollow();
        }
        followCoroutine = StartCoroutine(StartFollowRoutine(target, followDistance));
    }
    private IEnumerator StartFollowRoutine(Transform target, float followDistance)
    {
        currentTargetPos = target.position;
        isFollowing = true;
        Target = target;

        while (!Extensions.CheckRangeBetweenUnits(transform, target, followDistance))
        {
            ClientMove(target.position);
            yield return Extensions.GetWait(0.1f);
        }
        StopFollow();
    }
    public void StopFollow()
    {
        if (followCoroutine != null)
        {
            StopCoroutine(followCoroutine);
            followCoroutine = null;
        }
        isFollowing = false;
        ClientStop();
    }


    private bool CanMove()
    {
        if (movementBlockCount > 0) return false;
        if (controller.AttackController.IsAttacking) return false;
        if (GameFlowManager.Instance.CurrentState is GameState.GameEnd) return false;
        return true;

    }
}