using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    public List<Skill> possibleSkills;
    public Skill _skill;

    public void UnlockSkill(Skill skill)
    {
        _skill.SetController(transform);
    }
    public void UseSkill(Skill skill)
    {
        _skill.SkillStart();
    }
}
