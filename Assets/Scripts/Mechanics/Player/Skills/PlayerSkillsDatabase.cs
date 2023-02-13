using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerSkillsDatabase", menuName = "Scriptable Objects/PlayerSkillsDatabase", order = 1)]
public class PlayerSkillsDatabase : ScriptableSingleton<PlayerSkillsDatabase>
{
    public List<ClassSkillsPair> SkillLists = new List<ClassSkillsPair>();
}
[Serializable]
public class ClassSkillsPair
{
    public Class Class;
    public List<PlayerSkill> AllClassSkills;
}