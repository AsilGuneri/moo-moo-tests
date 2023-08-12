using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : NetworkBehaviour
{
    public Animator Animator { get => animator; }
    [SerializeField] private bool hasCancelAttackAnim;
    protected UnitController controller;
    protected Animator animator;
    private Dictionary<string, float> animationLenghts = new Dictionary<string, float>();
    public float AttackAnimTime ;//{ get; private set; }
    [SerializeField] private string attackAnimName;


    protected void Awake()
    {
        controller = GetComponent<UnitController>();
        animator = GetComponent<Animator>();
    }
    protected void Start()
    {
        CacheAnimationDurations();
    }
    public void SetAttackStatus(bool isAttacking)
    {
        if (animator) animator.SetBool("isAttacking", isAttacking);
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
        if (!hasCancelAttackAnim) return;
        animator.Play("CancelAttack");
    }

    private void CacheAnimationDurations()
    {
        if (!animator) return;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
        if (!ac) return;
        foreach (var animClip in ac.animationClips)
        {
            if (animationLenghts.ContainsKey(animClip.name)) continue;
            animationLenghts.Add(animClip.name, animClip.length);

            if (animClip.name == attackAnimName)
            {
                AttackAnimTime = animClip.length;
            }
        }
    }
}
