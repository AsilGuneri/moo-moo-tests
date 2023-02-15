using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : Singleton<SkillBar>
{
    public UISkill[] UISkills = new UISkill[4];

    public void OnSkillSet(int skillGrade, SkillData skillData)
    {
        UISkills[skillGrade].Image.sprite = skillData.Icon;
        UISkills[skillGrade].Button.onClick.RemoveAllListeners();
        var playerObj = UnitManager.Instance.GetPlayerController().gameObject; 
        UISkills[skillGrade].Button.onClick.AddListener(() => skillData.GetController(playerObj).UseSkill());
    }
    public void OnCooldownStart()
    {
        //BURDAAAAAN
    }
}
[Serializable]
public class UISkill
{
    public Image Image;
    public Button Button;
}