using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableUISkill : MonoBehaviour
{
    [SerializeField] Image skillIcon;
    [SerializeField] GameObject lockedIcon;
    [SerializeField] GameObject selectedIcon;
    [SerializeField] Button selectButton;
    bool isLocked; //not implemented yet
    SkillData skillData;


    public void Setup(SkillData skillData)
    {
        this.skillData = skillData;
        SetButton();
        SetIcons();
    }
    public void UnSelectSkill()
    {
        selectedIcon.SetActive(false);
        selectButton.interactable = true;
    }

    private void SetIcons()
    {
        skillIcon.sprite = skillData.Icon;
        selectedIcon.SetActive(false);
    }

    private void SetButton()
    {
        selectButton.onClick.RemoveAllListeners();
        selectButton.onClick.AddListener(() =>
        {
            SkillSelectionPanel.Instance.ResetSelectedGradeSkills(skillData);
            SetSelectEvent();
        });
        selectButton.interactable = true;
    }

    private void SetSelectEvent()
    {
        var skillController = UnitManager.Instance.GetPlayerController().GetComponent<PlayerSkillController>();
        skillController.SetSkill(skillData.Name);
        selectedIcon.SetActive(true);
        selectButton.interactable = false;
    }
}
