using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSelectionPanel : Singleton<SkillSelectionPanel>
{
    [SerializeField] RectTransform[] skillParents = new RectTransform[4];
    [SerializeField] SelectableUISkill UISkillPrefab;
    [SerializeField] GameObject panel;

    ClassData classSkills = new ClassData();
    Class characterClass;

    List<SelectableUISkill> firstGradeSkills = new List<SelectableUISkill>();
    List<SelectableUISkill> secondGradeSkills = new List<SelectableUISkill>();
    List<SelectableUISkill> thirdGradeSkills = new List<SelectableUISkill>();
    List<SelectableUISkill> fourthGradeSkills = new List<SelectableUISkill>();



    private void Start()
    {
        //SetClassSkills(); bu burda dursun buna dönücez
    }

    public void TogglePanel()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }
    public void ResetSelectedGradeSkills(SkillData skillData)
    {
        int grade = skillData.Grade;
        foreach(var skill in GetGradeSelectableSkills(grade))
        {
            skill.UnSelectSkill();
        }
        
    }
    private void SetClassSkills()
    {
        foreach (var skill in classSkills.AllClassSkills)
        {
            var selectableSkill = Instantiate(UISkillPrefab);
            CacheGradeSelectableSkills(selectableSkill, skill.SkillData.Grade);
            selectableSkill.Setup(skill.SkillData);
            selectableSkill.transform.SetParent(skillParents[skill.SkillData.Grade]);
        }


    }
    public void CacheClassSkills()
    {
        characterClass = UnitManager.Instance.GetPlayerController().CharacterClass;
        classSkills = PlayerSkillsDatabase.Instance.GetClassData(characterClass);
        SetClassSkills();
    }
    private void CacheGradeSelectableSkills(SelectableUISkill skill, int grade)
    {
        switch(grade) 
        {
            case 0:
                firstGradeSkills.Add(skill);
                break;
            case 1:
                secondGradeSkills.Add(skill);
                break;
            case 2:
                thirdGradeSkills.Add(skill);
                break;
            case 3:
                fourthGradeSkills.Add(skill);
                break;
        }
    }
    private List<SelectableUISkill> GetGradeSelectableSkills(int grade)
    {
        switch (grade)
        {
            case 0:
                return firstGradeSkills;
            case 1:
                return secondGradeSkills;
            case 2:
                return thirdGradeSkills;
            case 3:
                return fourthGradeSkills;
        }
        return null;
    }
}
