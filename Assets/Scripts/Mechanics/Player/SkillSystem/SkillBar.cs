using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class SkillBar : Singleton<SkillBar>
{
    public UISkill[] UISkills = new UISkill[4];

    public void OnSkillSet(int skillGrade, SkillData skillData)
    {
        UISkills[skillGrade].SkillIcon.sprite = skillData.Icon;
        UISkills[skillGrade].Button.onClick.RemoveAllListeners();
        var playerObj = UnitManager.Instance.GetPlayerController().gameObject; 
        UISkills[skillGrade].Button.onClick.AddListener(() => skillData.GetController(playerObj).UseSkill());
    }
    public void OnCooldownStart(int skillGrade, float cooldown)
    {
        float fillAmount = 1;
        DOTween.To(() => fillAmount, x => fillAmount = x, 0, cooldown).OnUpdate(() =>
        {
            UISkills[skillGrade].CooldownImage.fillAmount = fillAmount;
            string cooldownText = 
            UISkills[skillGrade].CooldownText.text = CooldownText(cooldown, fillAmount);
        }).OnComplete(() =>
        {
            UISkills[skillGrade].CooldownText.text = string.Empty;
        });
    }
    private string CooldownText(float cooldown, float fillAmount)
    {
        float time = cooldown * fillAmount;

        if (time < 1.0f)
        {
            return time.ToString("F1"); // return time with one decimal place
        }
        else
        {
            return time.ToString("F0"); // return time with no decimal places
        }
    }

}
[Serializable]
public class UISkill
{
    public Image SkillIcon;
    public Button Button;
    public Image CooldownImage;
    public TextMeshProUGUI CooldownText;
}