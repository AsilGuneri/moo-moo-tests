using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;



    public void OnMove()
    {
        animator.SetBool("Move", true);
    }
    public void OnStop()
    {
        animator.SetBool("Move", false);

    }
    public void OnAttackStart(float attackSpeed)
    {
        animator.SetFloat("ShootSpeed", attackSpeed);
        animator.SetBool("IsAttacking", true);
    }
    public void OnAttackEnd()
    {
        animator.SetBool("IsAttacking", false);
    }
}

