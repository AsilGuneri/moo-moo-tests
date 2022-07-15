using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private string _currentAnimState;

    public string CurrentAnimState
    {
        get => _currentAnimState;
        set => _currentAnimState = value;
    }
    public void Animate(string nextState, bool canCancel, bool canRestart = false)
    {

        AnimationManager.Instance.ChangeAnimationState(nextState, _currentAnimState, animator, canCancel, canRestart);
        _currentAnimState = nextState;
    }
}

