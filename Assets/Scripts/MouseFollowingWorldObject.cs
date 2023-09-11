using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseFollowingWorldObject : MonoBehaviour
{
    private Camera mainCamera;
    private bool isFollowing;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    public void StartFollowing()
    {
        isFollowing = true;
        StartCoroutine(FollowMousePos());
    }
    public void StopFollowing()
    {
        isFollowing = false;
    }
    private IEnumerator FollowMousePos()
    {
        while (isFollowing)
        {
            yield return Extensions.GetWait(0.1f);
            Vector2 mousePos = Mouse.current.position.ReadValue();
            var ray = mainCamera.ScreenPointToRay(mousePos);
            var hits = Physics.RaycastAll(ray, 100);
            RaycastHit? groundHit = hits.FirstOrDefault(hit => hit.collider.gameObject.layer == 6);
            if (groundHit.HasValue)
            {
                transform.position = groundHit.Value.point;
            }
        }
        
    }
}
