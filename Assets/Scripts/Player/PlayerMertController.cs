using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class PlayerMertController : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Animator animator;
    [SerializeField] private SelectIndicator indicators;
    [SerializeField] private BasicAttackController bac;
    [SerializeField] private PlayerAnimationController pac;

    private Camera mainCamera;
    private Health _hc;
    private Health _target;
    private RaycastHit _hitInfo;
    private bool _hasPath;

    #region Server
  /*  [Command]
    private void CmdMove(Vector3 pos)
    {
        ClientMove(pos);
    }*/


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
        if (!navMeshAgent.hasPath && !bac.Target)
        {
            if (pac.CurrentAnimState != "Idle") pac.Animate("Idle", false);
        }

        if (Input.GetMouseButtonDown(0)) HandleInputs(InputType.MouseLeft);
        if (Input.GetMouseButtonDown(1)) HandleInputs(InputType.MouseRight);
    }
    private void ClientMove(Vector3 pos)
    {
        navMeshAgent.SetDestination(pos);
        if (pac.CurrentAnimState != "Run") pac.Animate("Run", false);
        bac.Target = null;
        bac.IsAttacking = false;
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
                    _target = hc;
                }
                else
                {
                    _target = null;
                }
            }
        }
        switch (input)
        {
            case InputType.MouseLeft:
                HandleLeftClick(_target);
                break;
            case InputType.MouseRight:
                HandleRightClick(_target, _hitInfo.point);
                break;
        }
    }
    private void HandleRightClick(Health hc, Vector3 point)
    {
        if (hc && hc != _hc) BasicAttack(hc);
        else
        {
            ClientMove(point);

        }

    }
    private void HandleLeftClick(Health hc)
    {

        if (hc && hc != _hc) SelectUnit(hc);
        else DeselectUnit();
    }
    private void SelectUnit(Health hc)
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


    private void BasicAttack(Health hc)
    {
        SelectUnit(hc);
        bac.Target = hc;
    }
}

public enum InputType
{
    MouseLeft,
    MouseRight,
    Keyboard
}