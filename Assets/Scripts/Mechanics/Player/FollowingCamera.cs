using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MyBox;

public class FollowingCamera : MonoBehaviour
{
    public bool IsFollowing = true;

   // [SerializeField] private CinemachineVirtualCamera virtualCam;
    [SerializeField] private float cornerThickness;
    [SerializeField] private float cornerMovementSpeed;
    [Separator("Mouse Wheel Zoom")]
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private float scrollSpeed;

    private Transform target;
    private FollowPosition followPos;
    Camera mainCam;

    private void Awake()
    {
        mainCam = GetComponent<Camera>();
        followPos = GetComponent<FollowPosition>();
    }
    void LateUpdate()
    {
        ZoomInOut();
        if (Input.GetKeyDown(KeyCode.Y)) ToggleLock();
        if (IsFollowing) return;

        
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
    public void SetupCinemachine(Transform playerTransform)
    {
        //virtualCam.m_Follow = playerTransform;
        target = playerTransform;
        followPos.Setup(mainCam, target);
    }
    private void ToggleLock()
    {
        IsFollowing = !IsFollowing;
        if (IsFollowing)
        {
            followPos.enabled = true;
            // cinemachineVirtualCamera.m_Follow = PlayerFollower.transform;
            //virtualCam.enabled = false;
        }
        else
        {
            followPos.enabled = false;
            //followEmptyObj.transform.position = PlayerFollower.transform.position;
            //virtualCam.enabled = true;
        }
    }

    private void ZoomInOut()
    {
        //virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance -= Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed;
        //virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance = Mathf.Clamp(virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_CameraDistance, minDistance, maxDistance);
    }
}
