using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSkillController : MonoBehaviour
{
    private PlayerMertController PlayerController;
    private Dictionary<Class, List<PlayerSkill>> skillDictionary = new Dictionary<Class, List<PlayerSkill>>();
    private SkillBar SkillBarInstance;

    private void Awake()
    {
        PlayerController = GetComponent<PlayerMertController>();
        InitializeDictionary();

        foreach (var skillList in PlayerSkillsDatabase.instance.SkillLists)
        {
            if (skillList.Class == PlayerController.CharacterClass)
            {
                SetClassSkills(skillList);
            }
        }
        StartCoroutine(TestSkillBar());

    }
    IEnumerator TestSkillBar()
    {
        //Sceneleri birle?tirdi?inde buray? sil, bu test yöntemi
        yield return new WaitUntil(() => SkillBar.Instance != null);
        yield return new WaitForSeconds(3);
        SkillBarInstance = SkillBar.Instance;
        SetSkill("PiercingArrow");
    }
    public void SetSkill(string skillName)
    {
        var possibleSkills = skillDictionary[PlayerController.CharacterClass];
        foreach (var skill in possibleSkills)
        {
            if (skillName == skill.SkillData.Name) 
            {
                skill.SkillData.SetController(gameObject);
                PlayerController.PlayerSkills[skill.SkillData.Grade] = skill;
                SkillBarInstance.OnSkillSet(skill.SkillData.Grade, skill.SkillData);
            }
        }
    }
    private void SetClassSkills(ClassSkillsPair classSkills)
    {
        foreach (var skill in classSkills.AllClassSkills)
        {
            var currentSkills = skillDictionary[PlayerController.CharacterClass];
            currentSkills.Add(skill);
        }
    }
    private void InitializeDictionary()
    {
        List<PlayerSkill> playerSkills = new List<PlayerSkill>();
        skillDictionary.Add(PlayerController.CharacterClass, playerSkills);
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
