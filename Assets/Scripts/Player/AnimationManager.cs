using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(menuName = "ScriptableObjects/Singletons/Animation Manager")]
public class AnimationManager : ScriptableSingleton<AnimationManager>
{
    public string ChangeAnimationState(string newState, Animator animator, string currentState, AnimType type = AnimType.Bool)
    {
        if (currentState == newState) return currentState;
        foreach(var parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
            // if(!isLooped) 
        }
        switch (type)
        {
            case AnimType.Bool:
                animator.SetBool(newState, false);
                break;
            case AnimType.Trigger:
                animator.SetTrigger(newState);
                break;
        }
        animator.SetBool(newState, true);
        Debug.Log(currentState + " " + newState);
        return newState;
    }
    public void ChangeAnimationState(string newState, string currentState, Animator animator, bool canCancel)
    {
        if (!canCancel && newState == currentState) return;
        animator.CrossFade(newState, 0.2f);
    }
}
public enum AnimType
{
    Bool,
    Trigger
}
