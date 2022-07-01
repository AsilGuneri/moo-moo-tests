using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : ASkill
{
    public string Name;
    public ClassType Class;
    public bool IsActive;
    public int ManaCost;
    public string AnimationName;

    public Action OnSkillStart;
    public Action OnSkillEnd;

    public override void UseSkill()
    {
        OnSkillStart?.Invoke();
    }
}
