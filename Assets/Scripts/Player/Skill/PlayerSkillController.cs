using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillController : MonoBehaviour
{
    [SerializeField] private ClassType classType;
    [SerializeField] private List<SkillBase> skills = new List<SkillBase>();

    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UseSkill(string skillName)
    { 
        var skillToUse = GetSkill(skillName);
        skillToUse.UseSkill();
    }
    private SkillBase GetSkill(string skillName)
    {
        foreach (var skill in skills)
        {
            if (skillName == skill.Name) return skill;
        }
        return null;
    }
}
