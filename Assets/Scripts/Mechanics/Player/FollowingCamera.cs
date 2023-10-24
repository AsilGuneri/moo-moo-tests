using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;

public class FollowingCamera : MonoBehaviour
{
    public bool IsFollowing = true;

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

    private void Awake()
    {
        mainCam = GetComponent<Camera>();
    }
    private void Update()
    {
        if (!isActive) return;

        if (Input.GetKeyDown(KeyCode.Y)) ToggleLock();
        
    }
    void LateUpdate()
    {
        if (!isActive) return;
        //ZoomInOut();
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

    private void ZoomInOut()
    {
    }
}
