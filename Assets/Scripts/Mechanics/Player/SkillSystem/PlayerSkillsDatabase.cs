using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerSkillsDatabase", menuName = "Scriptable Objects/PlayerSkillsDatabase", order = 1)]
public class PlayerSkillsDatabase : ScriptableSingleton<PlayerSkillsDatabase>
{
    public List<ClassSkillsPair> SkillLists = new List<ClassSkillsPair>();

    public ClassSkillsPair GetClassSkills(Class characterClass)
    {
        foreach(var pair in SkillLists) 
        {
            if(pair.Class == characterClass) return pair;
        }
        return null;
    }
}
[Serializable]
public class ClassSkillsPair
{
    public Class Class;
    public List<PlayerSkill> AllClassSkills;
}