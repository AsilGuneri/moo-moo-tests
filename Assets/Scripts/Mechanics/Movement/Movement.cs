using UnityEngine;
using System;
using ProjectDawn.Navigation.Hybrid;
using static UnityEditor.PlayerSettings;
using System.Threading.Tasks;

public class Movement : MonoBehaviour
{
    public Action OnMoveStart;
    public Action OnMoveStop;

    public bool IsMoving { get { return isMoving; } }

    private UnitController controller;
    private bool isMoving = false;
    private Vector3 currentTargetPos;
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
    public void ClientMove(Vector3 pos)
    {
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

        while (!Extensions.CheckRange(target.position, transform.position, followDistance))
        {
            ClientMove(target.position);
            await Task.Delay(100);
        }
        //Vector3 directionToTarget = target.position - transform.position;
        //directionToTarget.Normalize();

        //// Calculate a point followDistance units along the direction vector,
        //// starting from the target's position.
        //Vector3 targetPosition = target.position - directionToTarget * followDistance;
        //ClientMove(targetPosition);
    }
}