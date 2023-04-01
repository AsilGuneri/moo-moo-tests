using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using Pathfinding;
using Pathfinding.RVO;
using UnityEditor.Rendering;

public class UnitMovementController : MonoBehaviour
{


    private TargetController targetController;
    private AnimationController animationController;
    private IAstarAI ai;

    public float rotationSpeed = 5f;

    private void Awake()
    {
        targetController = GetComponent<TargetController>();
        animationController = GetComponent<AnimationController>();
        ai = GetComponent<IAstarAI>();
    }
    

    public void ClientMove(Vector3 pos, bool movingToTarget = false, float stoppingDistance = 2f)
    {
        if (animationController != null)
        {
            animationController.OnAttackEnd();
            animationController.OnMove();
        }
        if (!movingToTarget) targetController.SyncTarget(null);
        ai.destination = pos;
      
    }



    public void ClientStop()
    {
        if (animationController != null) animationController.OnStop();

       
    }

}
