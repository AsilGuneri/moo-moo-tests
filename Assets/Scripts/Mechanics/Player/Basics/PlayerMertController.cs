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
    public IAstarAI AiMovement { get; private set; }
    public TargetController TargetController { get; private set; }
    public BasicRangedAttackController AttackController { get; private set; }
    public AnimationControllerBase _pac { get; private set; }
    public UnitMovementController Movement { get; private set; }
    public PlayerDataHolder _dataHolder { get; private set; }
    public InputKeysData _inputKeys { get; private set; }




    public PlayerStats Stats;
    public Class CharacterClass;

    public string PlayerName { get; set; }

    [SerializeField] private Animator animator;
    
    [SerializeField] private Transform rangeIndicator;
    [SerializeField] private Indicators clickIndicator;

    private Camera mainCamera;
    private Health _hc;
    private bool _isAttackClickMode;



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
        SkillSelectionPanel.Instance.CacheClassSkills();
    }
    private void Awake()
    {
        TargetController = GetComponent<TargetController>();
        AttackController = GetComponent<BasicRangedAttackController>();
        _pac = GetComponent<AnimationControllerBase>();
        Movement = GetComponent<UnitMovementController>();
        _hc = GetComponent<Health>();
        _inputKeys = GetComponent<PlayerDataHolder>().KeysData;
        AiMovement = GetComponent<IAstarAI>();
    }

    [ClientCallback]
    void Update()
    {
        if (!hasAuthority) return;
        if (IsCastingSkill) return;



        if (Input.GetKeyDown(_inputKeys.AttackKey)) IsAttackClickMode = true;

        if (Input.GetKeyDown(_inputKeys.SelectKey) || Input.GetKeyDown(_inputKeys.MoveKey)) 
            OnPointerInput();
        if (Input.GetKeyDown(_inputKeys.StopKey))
        {
            Movement.ClientStop();
            TargetController.Target = null;
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
            OnClickEnemy(hc);
        }
        else
        {
            MoveToPoint(hitInfo);
        }
    }

    private void OnClickEnemy(Health hc)
    {
        //Check if the enemy is in range
            //if not, move to the enemy
            //if yes, attack the enemy

        TargetController.SetTarget(hc.gameObject);
    }

    private void MoveToPoint(RaycastHit hitInfo)
    {
        TargetController.SetTarget(null);
        Vector3 newPoint = Extensions.CheckNavMesh(hitInfo.point);
        Movement.ClientMove(newPoint);
        clickIndicator.Setup(hitInfo.point, true);
    }

    private void OnAttackModeClick(RaycastHit hitInfo)
    {
        clickIndicator.Setup(hitInfo.point, false);
        var closestEnemy = UnitManager.Instance.GetClosestUnit(hitInfo.point, UnitType.WaveEnemy);
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

        TargetController.Target = closestEnemy;
        IsAttackClickMode = false;

        return;
    }
    private void SetRangeIndicator(bool isOn)
    {
        rangeIndicator.gameObject.SetActive(isOn);
        if(rangeIndicator.localScale != Vector3.one * AttackController.Range) rangeIndicator.localScale = Vector3.one * AttackController.Range;
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
