using Mirror;
using ProjectDawn.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : UnitController
{
    public SkillController JumpSkill { get => jumpSkill; }
    public PlayerGoldController GoldController { get; private set; }

    [SerializeField] private SkillController jumpSkill;
    [SerializeField] private LayerMask clickableLayerMask;
    [SerializeField] private GameObject moveIndicator;
    [SerializeField] private GameObject attackModeIndicator;
    public string PlayerName { get; set; }
    public PlayerStats Stats { get; private set; } = new();

    private Camera mainCamera;
    private bool isAttackClickMode;
    private bool isSkillIndicatorActive;
    private SkillController indicatorActiveSkill;

    public AgentBody agent;

    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        statController = GetComponent<StatController>();
        GoldController = GetComponent<PlayerGoldController>();
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Activate();
    }

    void Update()
    {
        if (!isOwned) return;
        if (Input.GetKeyDown(KeyCode.T))
            WaveManager.Instance.SpawnTestWave();
    }
    private void Activate()
    {
        if (!isOwned) return;
        
        if (isServer) //server
        {
            //ContributionPanel.Instance.AddPlayerContributionField(this);
        }
        if (isClient) // client (host is also a client)
        {
            GoldController.CmdAddGold(100);
            UnitManager.Instance.RegisterUnit(this);
            StartCharacter(); // everyone
            GetComponent<PlayerInput>().enabled = true;
            LocalPlayerUI.Instance.SkillBarUI.AssignSkills(this);
            //show skills on UI here
        }
        
    }

    private void StartCharacter()
    {
        mainCamera = Camera.main;
        Debug.Log("Main camera is set");
        CameraController.Instance.Setup(transform);
        statController.InitializeStats();
        SubscribeAnimEvents();
    }

    public void GetMousePositionRaycastInfo(out Ray ray, out RaycastHit[] hits)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        ray = mainCamera.ScreenPointToRay(mousePos);
        hits = Physics.RaycastAll(ray, 100, clickableLayerMask);
    }

    private void OnRayHit(RaycastHit[] hits)
    {
        Vector3 groundHitPos = default;
        GameObject clickedEnemy = null;
        foreach (var hitInfo in hits)
        {
            var obj = hitInfo.collider.gameObject;
            if (obj.layer == 9 && !clickedEnemy)
            {
                clickedEnemy = obj;
            }
            if (obj.layer ==  6) // ground
            {
                groundHitPos = hitInfo.point;
            }
        }
        if (clickedEnemy)
        {
            var clickedUnitType = clickedEnemy.GetComponent<UnitController>().unitType;
            if (IsEnemyTo(clickedUnitType))
            {
                OnClickEnemy(clickedEnemy.transform);
            }
        }
        else if(groundHitPos != default)
        {
            MoveToPoint(groundHitPos);
        }
    }

    private void OnClickEnemy(Transform enemyTransform)
    {
        StartAttack(enemyTransform);

    }
    private void StartAttack(Transform enemyTransform)
    {
        targetController.SetTarget(enemyTransform.GetComponent<NetworkIdentity>());
        //Check if the enemy is in range
        bool isInRange = Extensions.CheckRange(enemyTransform.position, transform.position, attackRange);
        if (isInRange) //if yes, attack the enemy
        {
            movement.ClientStop();
            attackController.StartAutoAttack();

        }
        else //if not, follow the enemy
        {
        }
    }
    private void OnAttackModeClick(Vector3 clickPos)
    {
        PrefabPoolManager.Instance.GetFromPool(attackModeIndicator.gameObject, Extensions.Vector3WithoutY(clickPos), attackModeIndicator.transform.rotation);
        var closestEnemy = UnitManager.Instance.GetClosestUnit(clickPos, UnitType.WaveEnemy);
        if (closestEnemy == null)
        {
            isAttackClickMode = false;
            return;
        }
        float maxDistanceBetweenPointAndUnit = 20; /*distance between the click and monster change that*/
        if (!Extensions.IsInRange(closestEnemy.transform.position, clickPos, maxDistanceBetweenPointAndUnit))
        {
            isAttackClickMode = false;
            return;
        }
        StartAttack(closestEnemy.transform);
        isAttackClickMode = false;
        return;
    }
    private void MoveToPoint(Vector3 point)
    {
        targetController.SetTarget(null);

        Vector3 newPoint = Extensions.CheckNavMesh(point);
        Movement.ClientMove(newPoint);
        PrefabPoolManager.Instance.GetFromPool(moveIndicator.gameObject, Extensions.Vector3WithoutY(newPoint), moveIndicator.transform.rotation);
    }

    private bool CanClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;
        if (!mainCamera) return false;
        return true;
    }

    #region Stats
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
    #endregion

    #region Input Events
    private void OnLeftClick() //used by input component
    {
         if (!isOwned) return;
        if (!CanClick()) return;
        Ray ray;
        RaycastHit[] hits;
        GetMousePositionRaycastInfo(out ray, out hits);
        RaycastHit ? groundHit = hits.FirstOrDefault(hit => hit.collider.gameObject.layer == 6);

        if (isAttackClickMode) //will handle that part later
        {
            if (groundHit.HasValue)
            {
                OnAttackModeClick(groundHit.Value.point);
                return;
            }
        }
        else if (isSkillIndicatorActive)
        {
            indicatorActiveSkill.EndIndicator();
            indicatorActiveSkill = null;
            isSkillIndicatorActive = false;
        }
        else if (hits.Length > 0)
        {
            OnRayHit(hits);
        }

    }

    private void OnRightClick()//used by input component
    {
        if (!isOwned) return;
        if (!CanClick()) return;
        Ray ray;
        RaycastHit[] hits;
        GetMousePositionRaycastInfo(out ray, out hits);
        RaycastHit? groundHit = hits.FirstOrDefault(hit => hit.collider.gameObject.layer == 6);
        if (isAttackClickMode) //will handle that part later
        {
            if (groundHit.HasValue)
            {
                OnAttackModeClick(groundHit.Value.point);
                return;
            }
        }
        else if (hits.Length > 0)
        {
            OnRayHit(hits);
        }
    }
    private void OnSetAutoAttackMode()//used by input component
    {
        if (!isOwned) return;
        isAttackClickMode = true;
    }
    private void OnSkill0()
    {
        var skill = skills[0];
        if (!CanCastSkill(skill)) return;
        if (skill.SkillData.HasIndicator)
        {
            skill.StartIndicator();
            isSkillIndicatorActive = true;
            indicatorActiveSkill = skill;
        }
        else skill.Use();
    }
    private void OnSkill1()
    {
        //UseSkillById(1);
    }
    private void OnSkill2()
    {
        //UseSkillById(2);
    }
    private void OnSkill3()
    {
        //UseSkillById(3);
    }
    private void OnFlash()
    {
        if (!CanCastSkill(jumpSkill)) return;
        var skill = jumpSkill;

        if (skill.SkillData.HasIndicator)
        {
            skill.StartIndicator();
            isSkillIndicatorActive = true;
            indicatorActiveSkill = skill;
        }
        else skill.Use();

    }
    #endregion
    private bool CanCastSkill(SkillController skill)
    {
        if (skill.OnCooldown) return false;
        if (skill.SkillData.ManaCost > health.CurrentMana) return false;
        return true;
    }
}
