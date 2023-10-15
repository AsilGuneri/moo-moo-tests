using DuloGames.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class LocalPlayerUI : NetworkSingleton<LocalPlayerUI>
{
    public HealthBarUI HealthBarUI;
    public ExpBarUI ExpBarUI;
    public SkillBarUI SkillBarUI;
    
}
[Serializable]
public class SkillBarUI
{
    [SerializeField] UISpellSlot[] uiSlots = new UISpellSlot[4];

    public void AssignSkills(PlayerController player)
    {

        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (player.Skills.Count -1 < i) break;
            uiSlots[i].Assign(player.Skills[i].SkillData.skillInfo);
        }
    }
}

[Serializable]
public class HealthBarUI
{
    [SerializeField] Image healthFill;
    [SerializeField] TextMeshProUGUI healthText;

    public void UpdateHealthBar(int maxHp, int newHp)
    {
        //if (!isOwned) return;
        healthFill.fillAmount = (float)((float)newHp / (float)maxHp);
        healthText.text = newHp.ToString();
    }
}
[Serializable]
public class ExpBarUI
{
    [SerializeField] Image expFill;



    public void UpdateExpBar(float currentValue, float maxValue)
    {
        float fillAmount = currentValue / maxValue;
        expFill.fillAmount = fillAmount;
    }
}
