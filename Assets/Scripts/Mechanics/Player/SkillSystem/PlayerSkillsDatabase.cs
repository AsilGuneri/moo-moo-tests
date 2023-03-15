using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerSkillsDatabase", menuName = "Scriptable Objects/PlayerSkillsDatabase", order = 1)]
public class PlayerSkillsDatabase : ScriptableSingleton<PlayerSkillsDatabase>
{
    public List<ClassData> ClassList = new List<ClassData>();

    public ClassData GetClassData(Class characterClass)
    {
        foreach(var data in ClassList) 
        {
            if(data.Class == characterClass) return data;
        }
        return null;
    }
    public ClassData GetClassData(int index)
    {
        return ClassList[index];
    }
}
[Serializable]
public class ClassData
{
    public Class Class;
    public Sprite ClassLobbySprite;
    public GameObject ClassPrefab;
    public List<PlayerSkill> AllClassSkills;
}