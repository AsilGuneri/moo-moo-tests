using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour, IAnimationController
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject particle;



    public void OnMove()
    {
        animator.SetBool("Run", true);
        animator.SetBool("Attack", false);
    }

    public void OnStop()
    {
       
    }

    public void OnAttackStart(float attackSpeed)
    {
        animator.SetBool("Attack", true);
        animator.SetBool("Run", false);
    }

    public void OnAttackEnd()
    {
        
    }
}
