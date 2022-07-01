using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class SkillManager : Singleton<SkillManager>
{
    private List<SkillBase> strSkills;
    private List<SkillBase> intSkills;
    private List<SkillBase> agiSkills;

    public List<SkillBase> GetClassSkills(ClassType classType)
    {
        switch (classType)
        {
            case ClassType.Str:
                return strSkills;      
            case ClassType.Int:
                return intSkills;
            case ClassType.Agi:
                return agiSkills;
        }
        Debug.LogError($"No skillset found with given class {classType}");
        return null;
    }
}

public enum ClassType
{
    Str,
    Int,
    Agi
}
