using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSkillController : MonoBehaviour
{
    private PlayerController playerController;
    private Dictionary<Class, List<PlayerSkill>> skillDictionary = new Dictionary<Class, List<PlayerSkill>>();
    private SkillBar SkillBarInstance; //Remember to check skillbarinstance after the merge of the scenes.

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        InitializeDictionary();

        foreach (var skillList in PlayerSkillsDatabase.Instance.ClassList)
        {
            if (skillList.Class == playerController.playerClass)
            {
                SetClassSkills(skillList);
            }
        }
    }
   
    public void SetSkill(string skillName)
    {
        var possibleSkills = skillDictionary[playerController.playerClass];
        foreach (var skill in possibleSkills)
        {
            if (skillName == skill.SkillData.Name) 
            {
                skill.SkillData.SetController(gameObject);
                playerController.PlayerSkills[skill.SkillData.Grade] = skill;
                SkillBar.Instance.OnSkillSet(skill.SkillData.Grade, skill.SkillData);
            }
        }
    }
    private void SetClassSkills(ClassData classSkills)
    {
        foreach (var skill in classSkills.AllClassSkills)
        {
            var currentSkills = skillDictionary[playerController.playerClass];
            currentSkills.Add(skill);
        }
    }
    private void InitializeDictionary()
    {
        List<PlayerSkill> playerSkills = new List<PlayerSkill>();
        skillDictionary.Add(playerController.playerClass, playerSkills);
    }
} 
[Serializable]
public class PlayerSkill
{
    public string Name;
    public SkillData SkillData;
    [NonSerialized] public bool IsUnlocked;

    public void UseSkill(GameObject playerObj)
    {
        //  if(!IsUnlocked) return;
        var controller = SkillData.GetController(playerObj);
        controller.UseSkill();
    }
}
