using Mirror;
using System.Collections;
using UnityEngine;

public abstract class Skill : NetworkBehaviour
{
    public string Name;
    public float CooldownTime;
    public bool TargetRequired;
    public bool IsFocusingSkill;
    public SkillEffect Effect;

    protected bool isOnCooldown = false;

    public abstract void Use(UnitController user, UnitController target = null);

    protected IEnumerator Cooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(CooldownTime);
        isOnCooldown = false;
    }
    protected virtual void OnInterrupt()
    {

    }
}
public abstract class SkillEffect : MonoBehaviour
{
    public abstract void Apply(UnitController source, UnitController target);
}
