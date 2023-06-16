using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : UnitController
{
    [SerializeField] private Indicator moveIndicator;
    [SerializeField] private Indicator attackModeIndicator;
    public string PlayerName { get; set; }
    public PlayerStats Stats { get; private set; } = new();
    [NonSerialized] public PlayerSkill[] PlayerSkills = new PlayerSkill[4];
    public Class playerClass;
    public InputKeysData _inputKeys { get; private set; }
    private Camera mainCamera;
    private bool isAttackClickMode;

    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        _inputKeys = GetComponent<PlayerDataHolder>().KeysData;

    }
    private void Start()
    {
        Activate();
        animationController.SetAttackSpeed(attackSpeed);
    }

    void Update()
    {
        //if (Input.GetKeyDown(_inputKeys.SelectKey) || Input.GetKeyDown(_inputKeys.MoveKey))
        //    OnPointerInput();
        if (Input.GetKeyDown(_inputKeys.SpawnWaveKey))
            WaveManager.Instance.SpawnTestWave();
    }
    private void Activate()
    {
        if (NetworkServer.active) //server 
        {
            //GoldManager.Instance.GameBank.AddBankAccount(this);
            //ContributionPanel.Instance.AddPlayerContributionField(this);
        }
        if (isLocalPlayer) //owner
        {
            mainCamera = Camera.main;
            mainCamera.GetComponent<FollowingCamera>().SetupCinemachine(transform);
            SubscribeAnimEvents();
            UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), UnitType.Player);
        }
    }

    [TargetRpc]
    public void OnRegister()
    {
        //SkillSelectionPanel.Instance.CacheClassSkills();
    }
    
    private void GetMousePositionRaycastInfo(out Ray ray, out bool isRayHit, out RaycastHit hitInfo)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        ray = mainCamera.ScreenPointToRay(mousePos);
        isRayHit = Physics.Raycast(ray, out hitInfo, 100);
    }
    private void OnRayHit(RaycastHit hitInfo)
    {
        if (IsEnemy(hitInfo))
        {
            OnClickEnemy(hitInfo);
        }
        else
        {
            MoveToPoint(hitInfo);
        }
    }
    private void OnClickEnemy(RaycastHit hitInfo)
    {
        Transform enemyTransform = hitInfo.transform;
        StartAttack(enemyTransform);
    }
    private void StartAttack(Transform enemyTransform)
    {
        targetController.SetTarget(enemyTransform.gameObject);
        //Check if the enemy is in range
        bool isInRange = Extensions.CheckRange(enemyTransform.position, transform.position, attackRange);
        if (isInRange) //if yes, attack the enemy
        {
            movement.ClientStop();
            attackController.StartAutoAttack(enemyTransform.transform.gameObject, attackSpeed, animAttackPoint);

        }
        else //if not, follow the enemy
        {
        }
    }
    private void OnAttackModeClick(RaycastHit hitInfo)
    {
        IndicatorManager.Instance.StartIndicator(attackModeIndicator.gameObject, Extensions.Vector3WithoutY(hitInfo.point), attackModeIndicator.transform.rotation);
        var closestEnemy = UnitManager.Instance.GetClosestUnit(hitInfo.point, UnitType.WaveEnemy);
        if (closestEnemy == null)
        {
            isAttackClickMode = false;
            return;
        }
        float maxDistanceBetweenPointAndUnit = 20; /*distance between the click and monster change that*/
        if (!Extensions.IsInRange(closestEnemy.transform.position, hitInfo.point, maxDistanceBetweenPointAndUnit))
        {

            isAttackClickMode = false;
            return;
        }
        StartAttack(closestEnemy.transform);
        isAttackClickMode = false;

        return;
    }
    private void MoveToPoint(RaycastHit hitInfo)
    {
        targetController.SetTarget(null);
        Vector3 newPoint = Extensions.CheckNavMesh(hitInfo.point);
        Movement.ClientMove(newPoint);
        IndicatorManager.Instance.StartIndicator(moveIndicator.gameObject, Extensions.Vector3WithoutY(newPoint), moveIndicator.transform.rotation);
        //clickIndicator.Setup(hitInfo.point, true);
    }
   
    private bool CanClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;
        if(!mainCamera) return false;
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
        if (!CanClick()) return;
        Ray ray;
        bool isRayHit;
        RaycastHit hitInfo;
        GetMousePositionRaycastInfo(out ray, out isRayHit, out hitInfo);
        if (isAttackClickMode) //will handle that part later
        {
            OnAttackModeClick(hitInfo);
            return;
        }

    }
    private void OnRightClick()//used by input component
    {
        if (!CanClick()) return;

        Ray ray;
        bool isRayHit;
        RaycastHit hitInfo;
        GetMousePositionRaycastInfo(out ray, out isRayHit, out hitInfo);

        if (isAttackClickMode) //will handle that part later
        {
            OnAttackModeClick(hitInfo);
            return;
        }
        if (isRayHit)
        {
            OnRayHit(hitInfo);
        }

    }
    private void OnSetAutoAttackMode()//used by input component
    {
        isAttackClickMode = true;
    }
    #endregion
}
