using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerBase : MonoBehaviour
{
    protected Animator animator;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void SetAutoAttackStatus(bool isOn)
    {
        animator.SetBool("OnAutoAttack", isOn);
    }
    public virtual void OnAttackToMove()
    {
            OnAttackEnd();
            OnMove();
    }
    public virtual void OnMove()
    {
        animator.SetBool("Move", true);
    }
    public virtual void OnStop()
    {
        animator.SetBool("Move", false);
    }
    public virtual void OnAttackStart(float attackSpeed)
    {
        animator.SetFloat("ShootSpeed", attackSpeed);
        animator.SetBool("IsAttacking", true);        
    }
    public virtual void OnAttackEnd()
    {
        if (animator != null) animator.SetBool("IsAttacking", false);
    }
}