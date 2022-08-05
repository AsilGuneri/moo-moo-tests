using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using MyBox;
using UnityEngine.SceneManagement;

public class PlayerMertController : NetworkBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private ClassType _classType;
    [SerializeField] private SkillTier _currentTier;
    [SerializeField] private KeyCode attackKey;
    [SerializeField] private float attackKeyRange;


    [SerializeField] private KeyCode[] skillKeys;

    private SpecialClickType _currentClickType;
    private Camera mainCamera;
    private Health _hc;
    private RaycastHit _hitInfo;
    private bool _hasPath;
    private bool _isAttackClickMode = false;

    private TargetController _tc;
    private BasicAttackController _bac;
    private PlayerAnimationController _pac;
    private UnitMovementController _umc;
    private PlayerSkillController _psc;
    private NavMeshAgent _navMeshAgent;

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
        _navMeshAgent = GetComponent<NavMeshAgent>();

    }

    [TargetRpc]
    public void Activate() {
        mainCamera = Camera.main;
        
        mainCamera.GetComponent<FollowingCamera>().target = transform;
        Debug.Log("Target Adı: " + transform.name);

        StartCoroutine("Wtf");
    }

    private IEnumerator Wtf(){
        yield return new WaitUntil(() => UnitManager.Instance != null );
        UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), UnitType.Player);

    }
    

    [ClientCallback]
    void Update()
    {
        if (!hasAuthority) return;
        if (!_navMeshAgent.hasPath && !_tc.HasTarget)
        {
            _pac.OnStop();
        }


        if (Input.GetKeyDown(attackKey)) _isAttackClickMode = true;
        

        if (Input.GetMouseButtonDown(0)) HandleInputs(InputType.MouseLeft);
        if (Input.GetMouseButtonDown(1)) HandleInputs(InputType.MouseRight);
        if (Input.GetKeyDown(KeyCode.S)) _umc.ClientStop();
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
        if(!mainCamera)
            return;
        if(input is InputType.MouseLeft && _isAttackClickMode)
        {
            var closestEnemy = UnitManager.Instance.GetClosestUnit(transform.position, true);

            if (!Extensions.IsInRange(closestEnemy.transform.position, transform.position, attackKeyRange))
            {

                _isAttackClickMode = false;
                return;
            }

            _tc.SyncTarget(closestEnemy);
            _tc.HasTarget = true;
            _isAttackClickMode = false;

            return;
        }


        if (input is InputType.MouseLeft || input is InputType.MouseRight)
        {
            _isAttackClickMode = false;
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
       // indicators.SetupIndicator(null, false);
    }
    private void SetSpecialClickType(SpecialClickType clickType) 
    {
        _currentClickType = clickType;
       
    }
}
[System.Serializable]
public class ClickTypeGroup
{
    public KeyCode Key;
    public SpecialClickType ClickType;
}
public enum InputType
{
    MouseLeft,
    MouseRight,
    Keyboard
}
public enum SpecialClickType
{
    Attack,
    Move,
    None
}