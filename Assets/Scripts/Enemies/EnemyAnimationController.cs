using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour, IAnimationController
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject particle;

    public void OnAttack()
    {
        animator.SetBool("Attack", true);
        animator.SetBool("Run", false);
       // particle.SetActive(false);
    }
    public void OnWalk()
    {
       
    }
    public void OnRun()
    {
        animator.SetBool("Run", true);
        animator.SetBool("Attack", false);
       // particle.SetActive(true);
    }
}
public interface IAnimationController
{
    void OnAttack();
    void OnWalk();
    void OnRun();
}
