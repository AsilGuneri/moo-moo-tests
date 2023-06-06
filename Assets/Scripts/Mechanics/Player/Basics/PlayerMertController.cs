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
using Pathfinding;

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
    private bool _isAttackClickMode;

    private IAstarAI aiMovement;
    private TargetController _tc;
    private BasicRangedAttackController _bac;
    private AnimationControllerBase _pac;
    private UnitMovementController _umc;
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
    private void Activate()
    {
        if (NetworkServer.active)
        {
            GoldManager.Instance.GameBank.AddBankAccount(this);
            ContributionPanel.Instance.AddPlayerContributionField(this);
        }
        if (isLocalPlayer)
        {
            mainCamera = Camera.main;
            mainCamera.GetComponent<FollowingCamera>().SetupCinemachine(transform);
            UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), UnitType.Player);
        }
    }
    [TargetRpc]
    public void OnRegister()
    {
        Debug.Log("asilxx" + UnitManager.Instance.Players.Count);
        SkillSelectionPanel.Instance.CacheClassSkills();
    }
    private void Awake()
    {
        _tc = GetComponent<TargetController>();
        _bac = GetComponent<BasicRangedAttackController>();
        _pac = GetComponent<AnimationControllerBase>();
        _umc = GetComponent<UnitMovementController>();
        _hc = GetComponent<Health>();
        _inputKeys = GetComponent<PlayerDataHolder>().KeysData;
        aiMovement = GetComponent<IAstarAI>();
    }

    [ClientCallback]
    void Update()
    {
        if (!hasAuthority) return;
        if (IsCastingSkill) return;



        if (Input.GetKeyDown(_inputKeys.AttackKey)) IsAttackClickMode = true;

        if (Input.GetKeyDown(_inputKeys.SelectKey)) OnPointerInput();
        if (Input.GetKeyDown(_inputKeys.MoveKey)) OnPointerInput();
        if (Input.GetKeyDown(_inputKeys.StopKey))
        {
            _umc.ClientStop();
            _tc.Target = null;
            _pac.OnAttackEnd();
        }
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
            _tc.SetTarget(hc.gameObject);
        }
        else
        {
            _tc.SetTarget(null);
            _umc.ClientMove(hitInfo.point);
            clickIndicator.Setup(hitInfo.point, true);
        }
    }

    private void OnAttackModeClick(RaycastHit hitInfo)
    {
        clickIndicator.Setup(hitInfo.point, false);
        var closestEnemy = UnitManager.Instance.GetClosestUnit(hitInfo.point, true);
        if (!closestEnemy)
        {
            IsAttackClickMode = false;
            return;
        }
        float maxDistanceBetweenPointAndUnit = 20; /*distance between the click and monster change that*/
        if (!Extensions.IsInRange(closestEnemy.transform.position, hitInfo.point, maxDistanceBetweenPointAndUnit))
        {

            IsAttackClickMode = false;
            return;
        }

        _tc.Target = closestEnemy;
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
