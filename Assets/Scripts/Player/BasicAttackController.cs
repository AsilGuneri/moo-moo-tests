using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;


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
    private float _timer = 0;
    private HealthController _hc;

    private void Start()
    {
        UpdateCooldown();
        OnAttackEnd.AddListener(EndBasicAttackAnim);
    }

    private void FixedUpdate()
    {
        if (_isCounting)
        {
            _timer+= Time.deltaTime;
            if (_timer >= _basicCoolDown) EndBasicCoolDown();
        }
    }
    public virtual void BasicAttack(HealthController hc, Animator animator, string lastState)
    {
        if (!_basicAttackReady) return;
        if (navMeshAgent.hasPath)
        {
            navMeshAgent.SetDestination(transform.position);
            AnimationManager.Instance.ChangeAnimationState("BasicAttack", animator, lastState);

        }
        _hc = hc;

        AnimationManager.Instance.ChangeAnimationState("BasicAttack", animator, lastState);
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
        _basicCoolDown =  1 / GetComponent<PlayerController>().AttackSpeed;
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

    }
    private void EndBasicAttackAnim()
    {
        GetComponent<Animator>().SetBool("BasicAttack", false);
    }

}
