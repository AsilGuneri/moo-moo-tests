using UnityEngine;
using System;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Sample.Scenarios;
using ProjectDawn.Navigation.Hybrid;

public class Movement : MonoBehaviour
{
    //public Action OnMoveStart;
    //public Action OnMoveStop;
    //public bool IsMoving { get { return isMoving; } }


    //[SerializeField] float rotationSpeed = 5f;

    //private UnitController controller;
    //private bool isMoving = false;
    private Transform currentTarget;
    private AgentDestinationAuthoring agent;

    private void Awake()
    {
        //controller = GetComponent<UnitController>();
        //aiMovement = GetComponent<RichAI>();
         agent = GetComponent<AgentDestinationAuthoring>();
    }

    private void Update()
    {
        //    if (isMoving)
        //    {
        //        //RotateTowardsSteeringTarget();

        //        // Only call ClientStop if the character reached the end of the path and has no target
        //       // if (ReachedDestination(0.5f) && controller.TargetController.Target == null)
        //       // {
        //       //     ClientStop();
        //      //  }
        //    }
    }
    public void ClientMove(Vector3 pos)
    {
        ////aiMovement.destination = pos;
        //isMoving = true;
        ////aiMovement.isStopped = false;
        //OnMoveStart?.Invoke();
        agent.SetDestination(pos);
    }

    public void ClientStop()
    {
        //isMoving = false;
        ////aiMovement.isStopped = true;
        //OnMoveStop?.Invoke();
        agent.Stop();
    }
    public void FollowTarget(Transform target, float followDistance)
    {

        //float distance = Extensions.Distance(transform.position, target.position);
        //if (distance <= followDistance)
        //{
        //    ClientStop();
        //    return;
        //}
        //Vector3 directionToTarget = target.position - transform.position;
        //directionToTarget.Normalize();

        //// Calculate a point followDistance units along the direction vector,
        //// starting from the target's position.
        //Vector3 targetPosition = target.position - directionToTarget * followDistance;

        ////aiMovement.Move(transform.position + directionToTarget * Time.deltaTime * aiMovement.maxSpeed); 
        //ClientMove(targetPosition);
    }
    private void SetDestination()
    {
        var agent = transform.GetComponent<AgentAuthoring>();
        var body = agent.EntityBody;
        body.Destination = currentTarget.position;
        body.IsStopped = false;
        agent.EntityBody = body;
    }
    //private bool ReachedDestination(float threshold)
    
        //if (aiMovement.destination != Vector3.zero)
        //{
        //    return Extensions.CheckRange(aiMovement.destination, transform.position, threshold);
        //}
        //return false;
    
}
