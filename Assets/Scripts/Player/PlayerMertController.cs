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

    private Camera mainCamera;
    private string _currentAnimState;
    private HealthController _hc;
    private HealthController _target;
    private RaycastHit _hitInfo;
    private bool _hasPath;

    private float _attackSpeed = 1; //Available attack per seconds

    public float AttackSpeed
    {
        get => _attackSpeed;
        set
        {
            _attackSpeed = value;
            bac.UpdateCooldown();
        }
    }
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
        _hc = GetComponent<HealthController>();
    }
    [ClientCallback]
    void Update()
    {
        if (!hasAuthority) return;
        if (!navMeshAgent.hasPath && (!bac.IsAttacking || _currentAnimState != "Shoot"))
        {
            if (_currentAnimState != "Idle") Animate("Idle", false);
        }

        if (Input.GetMouseButtonDown(0)) HandleInputs(InputType.MouseLeft);
        if (Input.GetMouseButtonDown(1)) HandleInputs(InputType.MouseRight);
    }
    private void ClientMove(Vector3 pos)
    {
        navMeshAgent.SetDestination(pos);
        if (_currentAnimState != "Run") Animate("Run", false);
        bac.IsAutoAttacking = false;
    }
    private void HandleInputs(InputType input)
    {
        if (input is InputType.MouseLeft || input is InputType.MouseRight)
        {
            Ray myRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay, out _hitInfo, 100))
            {
                if (_hitInfo.collider.TryGetComponent(out HealthController hc))
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
    private void HandleRightClick(HealthController hc, Vector3 point)
    {
        if (hc && hc != _hc) BasicAttack(hc);
        else
        {
            ClientMove(point);

        }

    }
    private void HandleLeftClick(HealthController hc)
    {

        if (hc && hc != _hc) SelectUnit(hc);
        else DeselectUnit();
    }
    private void SelectUnit(HealthController hc)
    {
        //if(I say no)
        //        you say PLEAASE
        indicators.SetupIndicator(hc.GetComponent<SelectIndicator>().StaticIndicator, true);

    }
    private void DeselectUnit()
    {
        indicators.SetupIndicator(null, false);
    }
    public void Animate(string nextState, bool canCancel)
    {
        AnimationManager.Instance.ChangeAnimationState(nextState, _currentAnimState, animator, canCancel);
        _currentAnimState = nextState;
    }
    #endregion


    private void BasicAttack(HealthController hc)
    {
        SelectUnit(hc);
        bac.BasicAttack(hc, _currentAnimState);

    }
}

public enum InputType
{
    MouseLeft,
    MouseRight,
    Keyboard
}