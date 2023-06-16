using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : NetworkBehaviour
{
    public Animator Animator { get => animator; }
    protected UnitController controller;
    protected Animator animator;


    protected void Awake()
    {
        controller = GetComponent<UnitController>();
        animator = GetComponent<Animator>();
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

    //make that class abstract and move that method to player
    public void SetAttackCancelled()
    {
        animator.Play("CancelAttack");
    }
}
