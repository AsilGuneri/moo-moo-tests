using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;


public class BasicAttackController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private NavMeshAgent navMeshAgent;

    public UnityEvent OnAttackStart;
    public UnityEvent OnAttackEnd;

    private int _basicDamage = 1;
    private float _basicCoolDown = 1f;
    private bool _basicAttackReady = true;
    private bool _isCounting = false;
    private bool _isAttacking = false;
    private bool _isAutoAttacking = false;
    private float _timer = 0;
    private HealthController _hc;
    private PlayerMertController _pc;

    public bool IsAttacking { get => _isAttacking; }
    public bool IsAutoAttacking 
    {
        get => _isAutoAttacking;
        set => _isAutoAttacking = value;
    }

    private void Start()
    {
       // if (!hasAuthority) return;

        UpdateCooldown();
        OnAttackEnd.AddListener(EndBasicAttack);
        _pc = GetComponent<PlayerMertController>();
    }

    private void FixedUpdate()
    {
        //if (!hasAuthority) return;

        if (_isCounting)
        {
            _timer+= Time.deltaTime;
            if (_timer >= _basicCoolDown) EndBasicCoolDown();
        }
    }
    public virtual void BasicAttack(HealthController hc, string lastState)
    {

        if (!_basicAttackReady) return;
        _isAutoAttacking = true;
        _isAttacking = true;
        if (navMeshAgent.hasPath)
        {
            navMeshAgent.SetDestination(transform.position);
            //AnimationManager.Instance.ChangeAnimationState("BasicAttack", animator, lastState);
            _pc.Animate("Shoot", true);

        }
        _hc = hc;

        //AnimationManager.Instance.ChangeAnimationState("BasicAttack", animator, lastState);
        _pc.Animate("Shoot", true);
        transform.LookAt(hc.transform);
        hc.TakeDamage(_basicDamage);
        DealDamage();
        StartBasicCooldown();
    }
    public void DealDamage()
    {
        var arrow = Instantiate(projectilePrefab, transform.position, Quaternion.identity).GetComponent<Projectile>();
        arrow.Target = _hc;
    }
    public void UpdateCooldown() // CALL THIS METHOD WHENEVER YOU CHANGE ATTACKSPEED
    {
        _basicCoolDown =  1 / GetComponent<PlayerMertController>().AttackSpeed;
    }

    public void InvokeAttackEnded()
    {
        OnAttackEnd?.Invoke();
    }
    public void InvokeAttackStart()
    {
        OnAttackStart?.Invoke();
    }

    private void StartBasicCooldown()
    {
        _isCounting = true;
        _basicAttackReady = false;
        _timer = 0;
    }
    private void EndBasicCoolDown()
    {
        _basicAttackReady = true;
        if (_isAutoAttacking) BasicAttack(_hc, "Shoot");
    }
    private void EndBasicAttack()
    {
        _isAttacking = false;
     //   StartCoroutine(nameof(RepeatBasicAttack));
    }
    private IEnumerator RepeatBasicAttack()
    {
        yield return new WaitUntil(() => _basicAttackReady);
        BasicAttack(_hc, "Shoot");
    }

}
