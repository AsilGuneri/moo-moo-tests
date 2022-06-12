using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class AnimationManager : Singleton<AnimationManager>
{
    public void ChangeAnimationState(string newState, string currentState, Animator animator, bool canCancel, bool canRestart)
    {
        if (!canCancel && newState == currentState) return;
        if (canRestart)
        {
            animator.Play("Idle", 0, 0);
            animator.CrossFade(newState, 0.01f, 0, 0);
        }
        else
        {
            animator.CrossFade(newState, 0f);
        }
    }

}
public enum AnimType
{
    Bool,
    Trigger
}
