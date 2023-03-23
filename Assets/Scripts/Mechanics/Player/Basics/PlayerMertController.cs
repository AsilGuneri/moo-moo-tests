using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using MyBox;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System;

public class PlayerMertController : NetworkBehaviour
{
    public PlayerStats Stats;
    public Class CharacterClass;
    public string PlayerName { get; set; }

    [SerializeField] private Animator animator;
    
    [SerializeField] private Transform rangeIndicator;
    [SerializeField] private Indicators clickIndicator;

    private Camera mainCamera;
    private Health _hc;
    private bool _hasPath;
    private bool _isAttackClickMode;

    private TargetController _tc;
    private BasicRangedAttackController _bac;
    private AnimationController _pac;
    private UnitMovementController _umc;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    private PlayerDataHolder _dataHolder;
    private InputKeysData _inputKeys;

    [NonSerialized] public PlayerSkill[] PlayerSkills = new PlayerSkill[4];

    public bool IsCastingSkill { get; set; } = false;
    public Animator Animator
    {
        get => animator;
    }

    public bool IsAttackClickMode
    {
        get { return _isAttackClickMode; }
        private set
        {
            _isAttackClickMode = value;
            SetRangeIndicator(value);
        }
    }
    private void Start()
    {
        Activate();
    }

    private void Awake()
    {
        _tc = GetComponent<TargetController>();
        _bac = GetComponent<BasicRangedAttackController>();
        _pac = GetComponent<AnimationController>();
        _umc = GetComponent<UnitMovementController>();
        _hc = GetComponent<Health>();
        _inputKeys = GetComponent<PlayerDataHolder>().KeysData;
    }
    public void Activate() 
    {
        if (hasAuthority)
        {
            mainCamera = Camera.main; //DO NOT GET THE CAMERA LIKE THAT, get a reference to the cam.
            mainCamera.GetComponent<FollowingCamera>().SetupCinemachine(transform);
            UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), UnitType.Player);
        }
        if (NetworkServer.active)
        {
            GoldManager.Instance.GameBank.AddBankAccount(this);
            ContributionPanel.Instance.AddPlayerContributionField(this);
        }

    }

    [ClientCallback]
    void Update()
    {
        if (!hasAuthority) return;
        if (IsCastingSkill) return;
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
             WaveManager.Instance.SpawnTestWave();
            
        }

        if(Input.GetKeyDown(KeyCode.Q) && PlayerSkills[0] != null) 
        { 
            PlayerSkills[0].UseSkill(gameObject); 
        }
        if (Input.GetKeyDown(KeyCode.W) && PlayerSkills[1] != null)
        {
            PlayerSkills[1].UseSkill(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.E) && PlayerSkills[2] != null)
        {
            PlayerSkills[2].UseSkill(gameObject);
        }
        if (Input.GetKeyDown(KeyCode.R) && PlayerSkills[3] != null)
        {
            PlayerSkills[3].UseSkill(gameObject);
        }
    }
    public void SetSkill(PlayerSkill skill)
    {
        skill.SkillData.SetController(gameObject);
        PlayerSkills[skill.SkillData.Grade] = skill;
    }

    private void OnPointerInput()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
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

    // Add methods for updating PlayerStats
    public void AddDamageDealt(int damage)
    {
        Stats.TotalDamageDealt += damage;
        ContributionPanel.Instance.CmdUpdateContribution();
    }

    public void AddHealAmount(int heal)
    {
        Stats.TotalHealAmount += heal;
        //ContributionPanel.Instance.UpdateContribution();
    }

    public void AddDamageTanked(int damage)
    {
        Stats.TotalDamageTanked += damage;
        //ContributionPanel.Instance.UpdateContribution();
    }
}
