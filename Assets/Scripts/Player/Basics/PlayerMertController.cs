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
    

    [SerializeField] private float attackRange;
    [SerializeField] private Transform rangeIndicator;
    [SerializeField] private SpriteRendererFadeOut clickIndicator;

    private SpecialClickType _currentClickType;
    private Camera mainCamera;
    private Health _hc;
    private RaycastHit _hitInfo;
    private bool _hasPath;
    private bool _isAttackClickMode;

    private TargetController _tc;
    private BasicAttackController _bac;
    private PlayerAnimationController _pac;
    private UnitMovementController _umc;
    private PlayerSkillController _psc;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private InputKeysData _inputKeys;


    public bool IsAttackClickMode
    {
        get { return _isAttackClickMode; }
        private set
        {
            _isAttackClickMode = value;
            SetRangeIndicator(value);
        }
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

    [TargetRpc]
    public void Activate() {
        mainCamera = Camera.main;
        mainCamera.GetComponent<FollowingCamera>().SetupCinemachine(transform);
        StartCoroutine(nameof(RegisterRoutine));
    }
    private IEnumerator RegisterRoutine()
    {
        yield return new WaitUntil(() => UnitManager.Instance != null);
        Debug.Log("Wtf");
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


        if (Input.GetKeyDown(_inputKeys.AttackKey)) IsAttackClickMode = true;
        

        if (Input.GetKeyDown(_inputKeys.SelectKey)) HandleInputs(InputType.MouseLeft);
        if (Input.GetKeyDown(_inputKeys.MoveKey)) HandleInputs(InputType.MouseRight);
        if (Input.GetKeyDown(_inputKeys.StopKey)) _umc.ClientStop();
        if (Input.GetKeyDown(_inputKeys.SpawnWaveKey))
        {
            WaveManager.Instance.SpawnWave(WaveManager.Instance.waves[0]);
        }
        if (Input.GetKeyDown(_inputKeys.SkillKeys[0])) _psc.UseSkill(0);
        if (Input.GetKeyDown(_inputKeys.SkillKeys[1])) _psc.UseSkill(1);
        if (Input.GetKeyDown(_inputKeys.SkillKeys[2])) _psc.UseSkill(2);
        if (Input.GetKeyDown(_inputKeys.SkillKeys[3])) _psc.UseSkill(3);

    }

    private void HandleInputs(InputType input)
    {
        if(!mainCamera)
            return;

        Ray myRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        bool rayHit = Physics.Raycast(myRay, out _hitInfo, 100);

        if (input is InputType.MouseLeft && IsAttackClickMode)
        {
            clickIndicator.Setup(_hitInfo.point, false);
            var closestEnemy = UnitManager.Instance.GetClosestUnit(transform.position, true);
            if (!closestEnemy) 
            {
                IsAttackClickMode = false;
                return;
            }
            if (!Extensions.IsInRange(closestEnemy.transform.position, transform.position, attackRange))
            {

                IsAttackClickMode = false;
                return;
            }

            _tc.SyncTarget(closestEnemy);
            _tc.HasTarget = true;
            IsAttackClickMode = false;

            return;
        }


        if (input is InputType.MouseLeft || input is InputType.MouseRight)
        {
            IsAttackClickMode = false;
            if (rayHit)
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
                break;
            case InputType.MouseRight:
                HandleRightClick(_hitInfo.point);
                break;
        }
    }
    private void HandleRightClick(Vector3 point)
    {
        if (!_tc.HasTarget)
        {
            _umc.ClientMove(point);
            clickIndicator.Setup(point, true);
        }
    }
    private void SetSpecialClickType(SpecialClickType clickType) 
    {
        _currentClickType = clickType;
       
    }
    private void SetRangeIndicator(bool isOn)
    {
        rangeIndicator.gameObject.SetActive(isOn);
        if(rangeIndicator.localScale != Vector3.one * _bac.Range) rangeIndicator.localScale = Vector3.one * _bac.Range;
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