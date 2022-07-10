using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using MyBox;

public class PlayerMertController : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private SelectIndicator indicators;


    private Camera mainCamera;
    private Health _hc;
    private RaycastHit _hitInfo;
    private bool _hasPath;

    [Separator("Script References")]
    [SerializeField] private TargetController tc;
    [SerializeField] private BasicAttackController bac;
    [SerializeField] private PlayerAnimationController pac;
    [SerializeField] private UnitMovementController umc;
    [SerializeField] private PlayerSkillController psc;


    private void Awake()
    {
        tc = GetComponent<TargetController>();
    }
    #region Server


    #endregion
    #region Client

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
        mainCamera.GetComponent<FollowingCamera>().target = transform;
        _hc = GetComponent<Health>();
    }
    [ClientCallback]
    void Update()
    {
        if (!hasAuthority) return;
        if (!navMeshAgent.hasPath && !tc.HasTarget)
        {
            if (pac.CurrentAnimState != "Idle") pac.Animate("Idle", false);
        }

        if (Input.GetMouseButtonDown(0)) HandleInputs(InputType.MouseLeft);
        if (Input.GetMouseButtonDown(1)) HandleInputs(InputType.MouseRight);
        if (Input.GetKeyDown(KeyCode.S))
        {
            psc.UnlockSkill(psc._skill);
            psc.UseSkill(psc._skill);
        }
    }

    private void HandleInputs(InputType input)
    {
        if (input is InputType.MouseLeft || input is InputType.MouseRight)
        {
            Ray myRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay, out _hitInfo, 100))
            {
                if (_hitInfo.collider.TryGetComponent(out Health hc))
                {
                    tc.SyncTarget(hc.gameObject);
                    tc.HasTarget = true;
                }
                else
                {
                    tc.SyncTarget(null);
                    tc.HasTarget = false;
                }
            }
        }
        switch (input)
        {
            case InputType.MouseLeft:
                HandleLeftClick(tc.Target);
                break;
            case InputType.MouseRight:
                HandleRightClick(_hitInfo.point);
                break;
        }
    }
    private void HandleRightClick(Vector3 point)
    {
        if (!tc.HasTarget) umc.ClientMove(point);
    }
    private void HandleLeftClick(GameObject target)
    {

        if (target && target != gameObject) SelectUnit(target);
        else DeselectUnit();
    }
    private void SelectUnit(GameObject target)
    {
        //if(I say no)
        //        you say PLEAASE
       // indicators.SetupIndicator(hc.GetComponent<SelectIndicator>().StaticIndicator, true);

    }
    private void DeselectUnit()
    {
        indicators.SetupIndicator(null, false);
    }

 

    #endregion
}

public enum InputType
{
    MouseLeft,
    MouseRight,
    Keyboard
}