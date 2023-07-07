using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Fireball : Skill
{
    public override void Use(UnitController user, UnitController target)
    {
        if (!isOnCooldown && target != null)
        {
            Effect.Apply(user, target);
            StartCoroutine(Cooldown());
        }
    }
}

