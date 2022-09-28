using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationController 
{
    void OnMove();
    void OnStop();
    void OnAttackStart(float attackSpeed);
    void OnAttackEnd();
    
}
