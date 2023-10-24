using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;

public class FollowingCamera : MonoBehaviour
{
    public bool IsFollowing = true;
    public Transform zoomTransform;

    [SerializeField] private float cornerThickness;
    [SerializeField] private float cornerMovementSpeed;
    [Separator("Mouse Wheel Zoom")]
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float scrollSpeed;

    private Transform target;
    Camera mainCam;
    Vector3 initialOffset;
    bool isActive;
    bool isZooming;
    private float targetDistance;

    private void Awake()
    {
        mainCam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.Y)) ToggleLock();

        if (Input.mouseScrollDelta.y != 0)
        {
            targetDistance = zoomTransform.localPosition.z + (Input.mouseScrollDelta.y * scrollSpeed);
            targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);
        }

        // Check if the current zoom position is far from the target or if mouse scrolling is happening
        isZooming = Mathf.Abs(zoomTransform.localPosition.z - targetDistance) > 0.01f || Input.mouseScrollDelta.y != 0;
    }
    void LateUpdate()
    {
        if (!isActive) return;
        if (isZooming)
        {
            ZoomInOut();
        }
        if (IsFollowing)
        {
            FollowTarget();
            return;
        }

        
        if (Input.mousePosition.x >= Screen.width - cornerThickness) 
        {
            transform.position += new Vector3(Time.deltaTime * cornerMovementSpeed, 0, 0);
        }
        else if(Input.mousePosition.x <= cornerThickness)
        {
            transform.position -= new Vector3(Time.deltaTime * cornerMovementSpeed, 0, 0);
        }

        if (Input.mousePosition.y >= Screen.height - cornerThickness)
        {
            transform.position += new Vector3(0, 0, Time.deltaTime * cornerMovementSpeed);

        }
        else if (Input.mousePosition.y <= cornerThickness)
        {
            transform.position -= new Vector3(0, 0, Time.deltaTime * cornerMovementSpeed);
        }
    }
    private void FollowTarget()
    {
        transform.position = target.position + initialOffset;
    }
    public void SetupCinemachine(Transform playerTransform)
    {
        target = playerTransform;
        initialOffset = transform.position - target.position;
        isActive = true;
    }
    private void ToggleLock()
    {
        IsFollowing = !IsFollowing;
       
    }

    [SerializeField]
    private float zoomSmoothness = 10.0f; // The higher the value, the faster the zoom. Adjust to taste.

  private void ZoomInOut()
{
    Vector3 currentPos = zoomTransform.localPosition;
    Vector3 targetPos = new Vector3(0, 0, targetDistance);

    zoomTransform.localPosition = Vector3.Lerp(currentPos, targetPos, Time.deltaTime * zoomSmoothness);
}

}
