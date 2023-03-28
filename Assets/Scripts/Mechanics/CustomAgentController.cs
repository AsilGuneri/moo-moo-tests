using UnityEngine;
using UnityEngine.AI;

public class CustomAgentController : MonoBehaviour
{
    private Vector3 targetPos;
    public float speed = 8f;
    public float rotationSpeed = 720f;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (targetPos != null)
        {
            agent.SetDestination(targetPos);
            MoveToTarget();
            RotateToTarget();
        }
    }

    public void SetTarget(Vector3 targetPos)
    {
        if(targetPos == Vector3.zero)
        {
            this.targetPos = transform.position;
            return;
        }
        this.targetPos = targetPos;
    }

    private void MoveToTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);
        if (distanceToTarget > agent.stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, agent.steeringTarget, speed * Time.deltaTime);
        }
    }

    private void RotateToTarget()
    {
        Vector3 direction = (agent.steeringTarget - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}