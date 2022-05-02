using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private NetworkIdentity netId;
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Camera cam;
    [SerializeField] private Animator animator;
    [SerializeField] private SelectIndicator indicators;
    [SerializeField] private BasicAttackController bac;


    private string _currentAnimState;
    private HealthController _hc;
    private HealthController _target;
    private RaycastHit _hitInfo;

    private float _attackSpeed = 1; //Available attack per seconds

    public float AttackSpeed 
    { 
        get => _attackSpeed;
        set 
        {
            _attackSpeed = value;
            bac.UpdateCooldown();
        }
    }

    void Start()
    {
        if (!netId.hasAuthority) return;
        cam.gameObject.SetActive(true);
        _hc = GetComponent<HealthController>();
    }

    void Update()
    {
        if (!netId.hasAuthority) return;

        if (!navMeshAgent.hasPath)
        {
            if (_currentAnimState != "Idle") _currentAnimState = AnimationManager.Instance.ChangeAnimationState("Idle", animator, _currentAnimState);
        }
        else
        {
            if (_currentAnimState != "Run") _currentAnimState = AnimationManager.Instance.ChangeAnimationState("Run", animator, _currentAnimState);
        }
        if (Input.GetMouseButtonDown(0)) HandleInputs(InputType.MouseLeft);
        if (Input.GetMouseButtonDown(1)) HandleInputs(InputType.MouseRight);
    }

    private void HandleInputs(InputType input)
    {
        if (input is InputType.MouseLeft || input is InputType.MouseRight)
        {
            Ray myRay = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(myRay, out _hitInfo, 100))
            {
                if (_hitInfo.collider.TryGetComponent(out HealthController hc))
                {
                    _target = hc;
                }
                else
                {
                    _target = null;
                }
            }
        }       
        switch (input)
        {
            case InputType.MouseLeft:
                HandleLeftClick(_target);
                break;
            case InputType.MouseRight:
                HandleRightClick(_target, _hitInfo.point);
                break;
        }
    }

    private void HandleRightClick(HealthController hc, Vector3 point)
    {   
        if(hc == null){
            Debug.Log("Hc Null");
        }
        if(hc == _hc){
            Debug.Log("Hc Esit");
        }
        if (hc && hc != _hc) BasicAttack(hc);
        else Move(point);
        
    }
    private void HandleLeftClick(HealthController hc)
    {

        if (hc && hc != _hc) SelectUnit(hc);
        else DeselectUnit();
    }

    private void BasicAttack(HealthController hc)
    {
        SelectUnit(hc);
        bac.BasicAttack(hc, animator, _currentAnimState);

    }
    private void Move(Vector3 pos)
    {
        navMeshAgent.SetDestination(pos);
        if (_currentAnimState != "Run") _currentAnimState = AnimationManager.Instance.ChangeAnimationState("Run", animator, _currentAnimState);
    }
   private void SelectUnit(HealthController hc)
    {
        //if(I say no)
        //        you say PLEAASE
        indicators.SetupIndicator(hc.GetComponent<SelectIndicator>().StaticIndicator, true);

    }
    private void DeselectUnit()
    {
        indicators.SetupIndicator(null, false);
    }
}

public enum InputType
{
    MouseLeft,
    MouseRight,
    Keyboard
}