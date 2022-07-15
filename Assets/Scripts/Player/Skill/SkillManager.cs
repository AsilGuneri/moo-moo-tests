using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SkillManager : Singleton<SkillManager>
{
    public List<Skill> RogueSkills = new List<Skill>();
    public List<Skill> MageSkills = new List<Skill>();
    public List<Skill> WarriorSkills = new List<Skill>();

    public List<Skill> GetSkillGroup(ClassType classType, SkillTier skillTier)
    {
        List<Skill> skillGroup = new List<Skill>();
        switch(classType)
        {
            case ClassType.Rogue:
                skillGroup = GetTierSkills(RogueSkills, skillTier);
                break;
            case ClassType.Mage:
                skillGroup = GetTierSkills(MageSkills, skillTier);
                break;
            case ClassType.Warrior:
                skillGroup = GetTierSkills(WarriorSkills, skillTier);
                break;
        }
        return skillGroup;
    }
    private List<Skill> GetTierSkills(List<Skill> skills, SkillTier skillTier)
    {
        List<Skill> selectedSkills = new List<Skill>();
        foreach(var skill in skills)
        {
            if(skill.SkillData.Tier == skillTier) selectedSkills.Add(skill);
        }
        return selectedSkills;
    }

}
public enum SkillTier
{
    Tier1,
    Tier2,
    Tier3,
    Tier4
}
public enum ClassType
{
    Rogue,
    Mage,
    Warrior,
}
