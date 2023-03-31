using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Pathfinding;

public class UnitMovementController : MonoBehaviour
{
    [Separator("Script References")]
    [SerializeField] private AnimationController bac;
    public float moveSpeed = 10;
    private TargetController tc;
    private Seeker seeker;
    private Path currentPath;
    private int currentWaypoint;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        tc = GetComponent<TargetController>();
        seeker = GetComponent<Seeker>();
    }

    public void ClientMove(Vector3 pos, bool movingToTarget = false, float stoppingDistance = 0)
    {
        if (bac != null)
        {
            bac.OnAttackEnd();
            bac.OnMove();
        }
        if (!movingToTarget) tc.SyncTarget(null);

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        seeker.StartPath(transform.position, pos, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
            moveCoroutine = StartCoroutine(MoveAlongPath());
        }
    }

    private IEnumerator MoveAlongPath()
    {
        if (currentPath != null)
        {
            float pickNextWaypointDist = 0.3f; // Increase this value to switch waypoints earlier

            while (currentWaypoint < currentPath.vectorPath.Count)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentPath.vectorPath[currentWaypoint], moveSpeed * Time.deltaTime);

                float distanceToWaypoint = Vector3.Distance(transform.position, currentPath.vectorPath[currentWaypoint]);
                if (distanceToWaypoint < pickNextWaypointDist)
                {
                    currentWaypoint++;
                }

                // Smoothly rotate to face the next waypoint
                if (currentWaypoint < currentPath.vectorPath.Count)
                {
                    Vector3 targetDirection = (currentPath.vectorPath[currentWaypoint] - transform.position).normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
                    float rotationSpeed = 10f; // Adjust this value to control the speed of rotation
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                yield return null;
            }

            currentPath = null;
            moveCoroutine = null;
        }
    }


    public void ClientStop()
    {
        if (bac != null) bac.OnStop();

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
}
