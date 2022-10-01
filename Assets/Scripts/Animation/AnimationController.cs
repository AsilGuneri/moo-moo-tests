using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationController : MonoBehaviour
{
    [SerializeField] protected Animator animator;
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
        animator.SetBool("IsAttacking", false);
    }
}
