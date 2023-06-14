using UnityEngine;
using Pathfinding;
using System;

public class Movement : MonoBehaviour
{
    private UnitController controller;
    private RichAI aiMovement;
    private bool isMoving = false;

    public Action OnMoveStart;
    public Action OnMoveStop;


    public bool IsMoving { get { return isMoving; } }

    public float rotationSpeed = 5f;

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
            if (ReachedDestination(0.5f) && !controller.TargetController.HasTarget)
            {
                ClientStop();
            }
        }
    }
    private bool ReachedDestination(float threshold)
    {
        if (aiMovement.destination != Vector3.zero)
        {
            Vector2 currentPosition2D = new Vector2(transform.position.x, transform.position.z);
            Vector2 destination2D = new Vector2(aiMovement.destination.x, aiMovement.destination.z);

            float distance = Vector2.Distance(currentPosition2D, destination2D);
            return distance <= threshold;
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
    public void ClientMove(Vector3 pos)
    {
        //dont forget the stopping distance part
        aiMovement.destination = pos;
        isMoving = true;
        aiMovement.isStopped = false;
        OnMoveStart?.Invoke();
    }
    public void ClientStop()
    {
        //mechanic
        isMoving = false;
        aiMovement.isStopped = true;
        OnMoveStop?.Invoke();
    }

}
