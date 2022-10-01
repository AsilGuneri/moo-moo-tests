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
    
    [SerializeField] private Transform rangeIndicator;
    [SerializeField] private SpriteRendererFadeOut clickIndicator;

    private Camera mainCamera;
    private Health _hc;
    private bool _hasPath;
    private bool _isAttackClickMode;

    private TargetController _tc;
    private BasicRangedAttackController _bac;
    private AnimationController _pac;
    private UnitMovementController _umc;
    private PlayerSkillController _psc;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private PlayerDataHolder _dataHolder;
    private InputKeysData _inputKeys;


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
        _bac = GetComponent<BasicRangedAttackController>();
        _pac = GetComponent<AnimationController>();
        _umc = GetComponent<UnitMovementController>();
        _psc = GetComponent<PlayerSkillController>();
        _hc = GetComponent<Health>();
        _inputKeys = GetComponent<PlayerDataHolder>().KeysData;
    }

    [TargetRpc]
    public void Activate() {
        mainCamera = Camera.main; //DO NOT GET THE CAMERA LIKE THAT, get a reference to the cam.
        mainCamera.GetComponent<FollowingCamera>().SetupCinemachine(transform);
        StartCoroutine(nameof(RegisterRoutine));
    }
    private IEnumerator RegisterRoutine()
    {
        yield return new WaitUntil(() => UnitManager.Instance != null);
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

        if (Input.GetKeyDown(_inputKeys.SelectKey)) OnPointerInput();
        if (Input.GetKeyDown(_inputKeys.MoveKey)) OnPointerInput();
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
    private void OnPointerInput()
    {
        if (!mainCamera) return;
        Ray ray;
        bool isRayHit;
        RaycastHit hitInfo;
        GetRayInfo(out ray, out isRayHit, out hitInfo);

        if (IsAttackClickMode)
        {
            OnAttackModeClick(hitInfo);
            return;
        }
        IsAttackClickMode = false;
        if (isRayHit)
        {
            OnRayHit(hitInfo);
        }

    }
    private void GetRayInfo(out Ray ray, out bool isRayHit, out RaycastHit hitInfo)
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        isRayHit = Physics.Raycast(ray, out hitInfo, 100);
    }
    private void OnRayHit(RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent(out Health hc) && !hitInfo.collider.TryGetComponent(out PlayerMertController mc))
        {
            _tc.SyncTarget(hc.gameObject);
            _tc.HasTarget = true;
        }
        else
        {
            _tc.SyncTarget(null);
            _tc.HasTarget = false;
        }

        if (!_tc.HasTarget)
        {
            _umc.ClientMove(hitInfo.point);
            clickIndicator.Setup(hitInfo.point, true);
        }
    }

    private void OnAttackModeClick(RaycastHit hitInfo)
    {
        clickIndicator.Setup(hitInfo.point, false);
        var closestEnemy = UnitManager.Instance.GetClosestUnit(transform.position, true);
        if (!closestEnemy)
        {
            IsAttackClickMode = false;
            return;
        }
        if (!Extensions.IsInRange(closestEnemy.transform.position, transform.position, _bac.Range))
        {

            IsAttackClickMode = false;
            return;
        }

        _tc.SyncTarget(closestEnemy);
        _tc.HasTarget = true;
        IsAttackClickMode = false;

        return;
    }
    private void SetRangeIndicator(bool isOn)
    {
        rangeIndicator.gameObject.SetActive(isOn);
        if(rangeIndicator.localScale != Vector3.one * _bac.Range) rangeIndicator.localScale = Vector3.one * _bac.Range;
    }
}
