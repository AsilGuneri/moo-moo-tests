using UnityEngine;
using Pathfinding;
using System;

public class Movement : MonoBehaviour
{
    public Action OnMoveStart;
    public Action OnMoveStop;

    public RichAI AiMovement { get => aiMovement; }
    public bool IsMoving { get { return isMoving; } }


    [SerializeField] float rotationSpeed = 5f;

    private UnitController controller;
    private RichAI aiMovement;
    private bool isMoving = false;


    private void Awake()
    {
        controller = GetComponent<UnitController>();
        aiMovement = GetComponent<RichAI>();
    }

    private void Update()
    {
        if (isMoving)
        {
            RotateTowardsSteeringTarget();

            // Only call ClientStop if the character reached the end of the path and has no target
            if (ReachedDestination(0.5f) && controller.TargetController.Target == null)
            {
                ClientStop();
            }
        }
    }
    public void ClientMove(Vector3 pos)
    {
        aiMovement.destination = pos;
        isMoving = true;
        aiMovement.isStopped = false;
        OnMoveStart?.Invoke();
    }

    public void ClientStop()
    {
        isMoving = false;
        aiMovement.isStopped = true;
        OnMoveStop?.Invoke();
    }
    public void FollowTarget(Transform target, float followDistance)
    {
        float distance = Extensions.Distance(transform.position, target.position);
        if (distance <= followDistance)
        {
            ClientStop();
            return;
        }
        Vector3 directionToTarget = target.position - transform.position;
        directionToTarget.Normalize();

        // Calculate a point followDistance units along the direction vector,
        // starting from the target's position.
        Vector3 targetPosition = target.position - directionToTarget * followDistance;

        //aiMovement.Move(transform.position + directionToTarget * Time.deltaTime * aiMovement.maxSpeed); 
        ClientMove(targetPosition);
    }

    private bool ReachedDestination(float threshold)
    {
        if (aiMovement.destination != Vector3.zero)
        {
            return Extensions.CheckRange(aiMovement.destination, transform.position, threshold);
        }
        return false;
    }

    private void RotateTowardsSteeringTarget()
    {
        if (aiMovement.steeringTarget != null && aiMovement.velocity.sqrMagnitude > 0.01f)
        {
            Vector3 direction = aiMovement.steeringTarget - transform.position;
            direction.y = 0; // Ensure rotation only occurs around the Y-axis
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

            // Keep the current X and Z rotations
            float currentRotationX = transform.rotation.eulerAngles.x;
            float currentRotationZ = transform.rotation.eulerAngles.z;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Apply the original X and Z rotations back to the transform
            Vector3 updatedRotation = transform.rotation.eulerAngles;
            updatedRotation.x = currentRotationX;
            updatedRotation.z = currentRotationZ;
            transform.rotation = Quaternion.Euler(updatedRotation);
        }
    }
}
