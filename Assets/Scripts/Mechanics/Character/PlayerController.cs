using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : UnitController
{
    public InputKeysData _inputKeys { get; private set; }
    private Camera mainCamera;

    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        _inputKeys = GetComponent<PlayerDataHolder>().KeysData;
    }
    private void Start()
    {
        Activate();
    }

    void Update()
    {
        if (Input.GetKeyDown(_inputKeys.SelectKey) || Input.GetKeyDown(_inputKeys.MoveKey))
            OnPointerInput();
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
            UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(gameObject.GetComponent<NetworkIdentity>()), UnitType.Player);
        }
    }
    [TargetRpc]
    public void OnRegister()
    {
        //SkillSelectionPanel.Instance.CacheClassSkills();
    }
    private void OnPointerInput()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (!mainCamera) return;

        Ray ray;
        bool isRayHit;
        RaycastHit hitInfo;
        GetMousePositionRaycastInfo(out ray, out isRayHit, out hitInfo);

        //if (IsAttackClickMode) //will handle that part later
        //{
        //    OnAttackModeClick(hitInfo);
        //    return;
        //}
        //IsAttackClickMode = false;
        if (isRayHit)
        {
            OnRayHit(hitInfo);
        }

    }
    private void GetMousePositionRaycastInfo(out Ray ray, out bool isRayHit, out RaycastHit hitInfo)
    {
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
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
        targetController.SetTarget(enemyTransform.gameObject);
        //Check if the enemy is in range
        bool isInRange = Extensions.CheckRange(enemyTransform.position, transform.position, attackRange);
        if (isInRange) //if yes, attack the enemy
        {
            movement.ClientStop();
            attackController.StartAutoAttack(hitInfo.transform.gameObject, attackSpeed, animAttackPoint);
        }
        else //if not, follow the enemy
        {
        }
    }
    private void MoveToPoint(RaycastHit hitInfo)
    {
        targetController.SetTarget(null);
        Vector3 newPoint = Extensions.CheckNavMesh(hitInfo.point);
        Movement.ClientMove(newPoint);
        //clickIndicator.Setup(hitInfo.point, true);
    }
}
