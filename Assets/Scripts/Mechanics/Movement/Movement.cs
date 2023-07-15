using UnityEngine;
using System;
using ProjectDawn.Navigation.Hybrid;
using System.Threading.Tasks;
using static UnityEditor.PlayerSettings;

public class Movement : MonoBehaviour
{
    public Action OnMoveStart;
    public Action OnMoveStop;
    public Action OnFollowStop;

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
    public void SetDestinationOnAvailable(Vector3 pos, bool cancelTarget = false)
    {
        if (isFollowing)
        {
            StopFollow();
            OnFollowStop += (() => MoveOnFollowEnd(pos, cancelTarget));
        }
        else
        {
            ClientMove(pos, cancelTarget);
        }
    }
    public void ClientMove(Vector3 pos, bool cancelTarget = false)
    {
        Debug.Log($"asilxx move {name}  {StackTraceUtility.ExtractStackTrace()}");
        if (!CanMove()) return;
        if (cancelTarget) controller.TargetController.SetTarget(null);
        agent.SetDestination(pos);
        isMoving = true;
        currentTargetPos = pos;
        OnMoveStart?.Invoke();
    }

    public void ClientStop()
    {
        Debug.Log($"asilxx stop {name}  {StackTraceUtility.ExtractStackTrace()}");
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
            if (!isFollowing) return;
            ClientMove(target.position);
            await Task.Delay(100);
        }
        StopFollow();

    }
    public void StopFollow()
    {
        isFollowing = false;
        ClientStop();
        OnFollowStop?.Invoke();
    }

    private bool CanMove()
    {
        if (movementBlockCount > 0) return false;
        return !controller.AttackController.IsSetToStopAfterAttack;

    }
    private void MoveOnFollowEnd(Vector3 pos, bool cancelTarget)
    {
        ClientMove(pos, cancelTarget);
        OnFollowStop -= (() => MoveOnFollowEnd(pos,cancelTarget));
    }

}