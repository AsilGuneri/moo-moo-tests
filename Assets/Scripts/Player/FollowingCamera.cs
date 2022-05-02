using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    public Transform target;
    // Start is called before the first frame update
    private void Awake()
    {
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
      //  if (!target) return;
       // transform.position = target.position + offset;
    }
}
