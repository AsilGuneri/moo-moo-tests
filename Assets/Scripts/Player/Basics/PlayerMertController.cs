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

    private TargetController _tc;
    private BasicAttackController _bac;
    private PlayerAnimationController _pac;
    private UnitMovementController _umc;
    private PlayerSkillController _psc;


    private void Awake()
    {
        _tc = GetComponent<TargetController>();
        _bac = GetComponent<BasicAttackController>();
        _pac = GetComponent<PlayerAnimationController>();
        _umc = GetComponent<UnitMovementController>();
        _psc = GetComponent<PlayerSkillController>();
    }
    private void Start()
    {
        UnitManager.Instance.RegisterAllyUnits(gameObject);
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
        if (!navMeshAgent.hasPath && !_tc.HasTarget)
        {
            if (_pac.CurrentAnimState != "Idle") _pac.Animate("Idle", false);
        }

        if (Input.GetMouseButtonDown(0)) HandleInputs(InputType.MouseLeft);
        if (Input.GetMouseButtonDown(1)) HandleInputs(InputType.MouseRight);
        if (Input.GetKeyDown(KeyCode.S))
        {
            WaveManager.Instance.SpawnWave(WaveManager.Instance.waves[0]);
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
                    _tc.SyncTarget(hc.gameObject);
                    _tc.HasTarget = true;
                }
                else
                {
                    _tc.SyncTarget(null);
                    _tc.HasTarget = false;
                }
            }
        }
        switch (input)
        {
            case InputType.MouseLeft:
                HandleLeftClick(_tc.Target);
                break;
            case InputType.MouseRight:
                HandleRightClick(_hitInfo.point);
                break;
        }
    }
    private void HandleRightClick(Vector3 point)
    {
        if (!_tc.HasTarget) _umc.ClientMove(point);
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