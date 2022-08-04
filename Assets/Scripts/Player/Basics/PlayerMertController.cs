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

    [SerializeField] private ClassType _classType;
    [SerializeField] private SkillTier _currentTier;

    [SerializeField] private KeyCode[] skillKeys;

    private Camera mainCamera;
    private Health _hc;
    private RaycastHit _hitInfo;
    private bool _hasPath;

    private TargetController _tc;
    private BasicAttackController _bac;
    private PlayerAnimationController _pac;
    private UnitMovementController _umc;
    private PlayerSkillController _psc;

    public ClassType ClassType { get { return _classType; } }
    public SkillTier CurrentTier 
    {
        get { return _currentTier; } 
        set { _currentTier = value; }
    }

    private void Awake()
    {
        _tc = GetComponent<TargetController>();
        _bac = GetComponent<BasicAttackController>();
        _pac = GetComponent<PlayerAnimationController>();
        _umc = GetComponent<UnitMovementController>();
        _psc = GetComponent<PlayerSkillController>();
        _hc = GetComponent<Health>();


    }
    #region Server

    #endregion
    #region Client

    public override void OnStartAuthority()
    {
        mainCamera = Camera.main;
        mainCamera.GetComponent<FollowingCamera>().target = transform;
    }

    [ClientCallback]
    void Update()
    {
        if (!hasAuthority) return;
        if (!navMeshAgent.hasPath && !_tc.HasTarget)
        {
            _pac.OnStop();
        }

        if (Input.GetMouseButtonDown(0)) HandleInputs(InputType.MouseLeft);
        if (Input.GetMouseButtonDown(1)) HandleInputs(InputType.MouseRight);
        if (Input.GetKeyDown(KeyCode.S)) _umc.ClientStop();
        if (Input.GetKeyDown(KeyCode.PageDown)){
            Debug.Log("Player Count: " + UnitManager.Instance.Players.Count);
            Debug.Log("Player 0 Name: " + UnitManager.Instance.Players[0].Value.gameObject.name);
            Debug.Log("Player 1 Name: " + UnitManager.Instance.Players[1].Value.gameObject.name);
        } 
        if (Input.GetKeyDown(KeyCode.E))
        {
            WaveManager.Instance.SpawnWave(WaveManager.Instance.waves[0]);
        }
        if (Input.GetKeyDown(skillKeys[0])) _psc.UseSkill(0);
        if (Input.GetKeyDown(skillKeys[1])) _psc.UseSkill(1);
        if (Input.GetKeyDown(skillKeys[2])) _psc.UseSkill(2);
        if (Input.GetKeyDown(skillKeys[3])) _psc.UseSkill(3);

    }

    private void HandleInputs(InputType input)
    {
        if (input is InputType.MouseLeft || input is InputType.MouseRight)
        {
            Ray myRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay, out _hitInfo, 100))
            {
                if (_hitInfo.collider.TryGetComponent(out Health hc) && !_hitInfo.collider.TryGetComponent(out PlayerMertController mc))
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

    public override void OnStartClient()
    {
        base.OnStartClient();
        UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), UnitType.Player);
    }

    public override void OnStopClient()
    {
        UnitManager.Instance.UnregisterUnits(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), UnitType.Player);
        base.OnStopClient();
    }


 

    #endregion
}

public enum InputType
{
    MouseLeft,
    MouseRight,
    Keyboard
}