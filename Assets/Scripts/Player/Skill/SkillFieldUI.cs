using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SkillFieldUI : MonoBehaviour
{
    [SerializeField] private Image _skillImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Button _selectButton;


    public void SetSkillField(PlayerSkillController psc, Skill skill)
    {
        _skillImage.sprite = skill.SkillData.SkillImage;
        _nameText.text = skill.SkillData.Name;
        _descriptionText.text = skill.SkillData.Description;
        _selectButton.onClick.AddListener(delegate { psc.UnlockSkill(skill); });
    }
    

}
