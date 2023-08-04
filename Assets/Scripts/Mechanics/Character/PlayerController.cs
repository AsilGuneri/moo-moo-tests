using Mirror;
using ProjectDawn.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : UnitController
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private GameObject moveIndicator;
    [SerializeField] private GameObject attackModeIndicator;
    public string PlayerName { get; set; }
    public PlayerStats Stats { get; private set; } = new();

    public InputKeysData _inputKeys { get; private set; }
    private Camera mainCamera;
    private bool isAttackClickMode;

    public AgentBody agent;

    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        _inputKeys = GetComponent<PlayerDataHolder>().KeysData;

    }
    protected override void Start()
    {
        base.Start();
        Activate();
    }

    void Update()
    {
        if (Input.GetKeyDown(_inputKeys.SpawnWaveKey))
            WaveManager.Instance.SpawnTestWave();

       

        // Check if the Q key is pressed.
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    if (skills[0].TargetRequired)
        //    {
        //        GameObject target = null;

        //        Ray ray;
        //        bool isRayHit;
        //        RaycastHit hitInfo;
        //        GetMousePositionRaycastInfo(out ray, out isRayHit, out hitInfo);
        //        if(IsEnemy(hitInfo)) target = hitInfo.transform.gameObject;
        //        else return;
                
        //        // Use the first skill on a target.
        //       // skills[0].Use(this, target.GetComponent<UnitController>());
        //    }
        //}
    }
    private void Activate()
    {
        if (NetworkServer.active) //server 
        {
            //GoldManager.Instance.GameBank.AddBankAccount(this);
            //ContributionPanel.Instance.AddPlayerContributionField(this);
        }
        if (hasAuthority) //owner
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
        isRayHit = Physics.Raycast(ray, out hitInfo, 100, layerMask);
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
        ObjectPooler.Instance.SpawnFromPool(attackModeIndicator.gameObject, Extensions.Vector3WithoutY(hitInfo.point), attackModeIndicator.transform.rotation);
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
        ObjectPooler.Instance.SpawnFromPool(moveIndicator.gameObject, Extensions.Vector3WithoutY(newPoint), moveIndicator.transform.rotation);
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
