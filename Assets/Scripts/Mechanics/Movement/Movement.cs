using UnityEngine;
using System;
using ProjectDawn.Navigation.Hybrid;
using System.Threading.Tasks;

public class Movement : MonoBehaviour
{
    public Action OnMoveStart;
    public Action OnMoveStop;

    public bool IsMoving { get { return isMoving; } }

    private UnitController controller;
    private bool isMoving = false;
    private Vector3 currentTargetPos;
    private int movementBlockCount = 0;
    private bool isFollowing = false;
    AgentAuthoring agent;



    private void Awake()
    {
        agent = transform.GetComponent<AgentAuthoring>();
        controller = GetComponent<UnitController>();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S)) ClientStop();
        if (!isMoving) return;

        if (ReachedDestination(0.5f) && controller.TargetController.Target == null)
        {
            ClientStop();
        }
    }
    public void BlockMovement()
    {
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
        agent.SetDestination(transform.position);
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
    public async void StartFollow(Transform target, float followDistance)
    {
        currentTargetPos = target.position;
        isFollowing = true;
        while (!Extensions.CheckRange(target.position, transform.position, followDistance))
        {
            ClientMove(target.position);
            if (!isFollowing) break;
            await Task.Delay(100);
        }
        ClientStop();
    }
    public void StopFollow()
    {
        isFollowing = false;
    }
    private bool CanMove()
    {
        if (movementBlockCount > 0) return false;
        return !controller.AttackController.IsSetToStopAfterAttack;

    }

}