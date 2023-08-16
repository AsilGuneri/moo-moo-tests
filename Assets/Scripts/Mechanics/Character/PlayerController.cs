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
    [SerializeField] private LayerMask clickableLayerMask;
    [SerializeField] private GameObject moveIndicator;
    [SerializeField] private GameObject attackModeIndicator;
    public string PlayerName { get; set; }
    public PlayerStats Stats { get; private set; } = new();
    public EventVfx EventVfx { get; private set; }

    public InputKeysData _inputKeys { get; private set; }
    private Camera mainCamera;
    private bool isAttackClickMode;

    public AgentBody agent;

    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        _inputKeys = GetComponent<PlayerDataHolder>().KeysData;
        EventVfx = GetComponent<EventVfx>();

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

    private void GetMousePositionRaycastInfo(out Ray ray, out RaycastHit[] hits)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        ray = mainCamera.ScreenPointToRay(mousePos);
        hits = Physics.RaycastAll(ray, 100, clickableLayerMask);
    }

    private void OnRayHit(RaycastHit[] hits)
    {
        float minDistance = float.MaxValue;
        ClickableArea closestClickableArea = null;
        Vector3 groundHitPos = default;

        foreach (var hitInfo in hits)
        {
            if(hitInfo.collider.gameObject.layer ==  6) // ground
            {
                groundHitPos = hitInfo.point;
                continue;
            }

            var clickableArea = hitInfo.collider.GetComponent<ClickableArea>();
            if (clickableArea)
            {
                float distance = Vector3.Distance(hitInfo.point, clickableArea.ClickableCollider.bounds.center);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestClickableArea = clickableArea;
                }
            }
        }

        if (closestClickableArea)
        {
            var clickedUnitType = closestClickableArea.gameObject.GetComponent<UnitController>().unitType;
            if (IsEnemyTo(clickedUnitType))
            {
                OnClickEnemy(closestClickableArea.transform);
                Debug.Log("Clicked on enemy: " + closestClickableArea.gameObject.name);
            }
            // Use the closest clickable area.
            // Replace this with your own logic.
        }
        else
        {
            MoveToPoint(groundHitPos);
            // Perform move logic
            // MoveToPoint(hitInfo);
        }
    }

    private void OnClickEnemy(Transform enemyTransform)
    {
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
            attackController.StartAutoAttack(enemyTransform.transform.gameObject, attackSpeed);

        }
        else //if not, follow the enemy
        {
        }
    }
    private void OnAttackModeClick(Vector3 clickPos)
    {
        ObjectPooler.Instance.SpawnFromPool(attackModeIndicator.gameObject, Extensions.Vector3WithoutY(clickPos), attackModeIndicator.transform.rotation);
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
        else if (hits.Length > 0)
        {
            OnRayHit(hits);
        }

    }

    private void OnRightClick()//used by input component
    {
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
        isAttackClickMode = true;
    }
    #endregion
}
