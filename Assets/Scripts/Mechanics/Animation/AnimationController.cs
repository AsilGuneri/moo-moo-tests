using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : NetworkBehaviour
{
    public Animator Animator { get => animator; }
    protected NetworkAnimator networkAnimator;
    protected UnitController controller;
    protected Animator animator;


    protected void Awake()
    {
        controller = GetComponent<UnitController>();
        animator = GetComponent<Animator>();
        networkAnimator = GetComponent<NetworkAnimator>();
    }
    public void SetAttackStatus(bool isAttacking)
    {
        animator.SetBool("isAttacking", isAttacking);
    }
    public void SetMoveStatus(bool isMoving)
    {
        animator.SetBool("isMoving", isMoving);
    }
    public void SetAttackSpeed(float attackSpeed)
    {
        animator.SetFloat("attackSpeed", attackSpeed);
    }
    public void SetAttackCancelled()
    {
        animator.Play("CancelAttack");
    }
}
