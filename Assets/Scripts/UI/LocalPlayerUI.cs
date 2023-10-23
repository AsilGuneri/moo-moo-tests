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
    public GoldUI GoldUI;
    public InventoryUI InventoryUI;
}
[Serializable]
public class SkillBarUI
{
    [SerializeField] UISpellSlot[] uiSlots = new UISpellSlot[4];

    public void AssignSkills(PlayerController player)
    {

        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (player.Skills.Count - 1 < i) break;
            var skillController = player.Skills[i];
            uiSlots[i].Assign(skillController.SkillData.skillInfo);
            skillController.SetUISlot(uiSlots[i]);
        }
    }
}
[Serializable]
public class HealthBarUI
{
    [SerializeField] Image healthFill;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] Image manaFill;
    [SerializeField] TextMeshProUGUI manaText;


    public void UpdateHealthBar(int maxHealth, int newHealth)
    {
        //if (!isOwned) return;
        healthFill.fillAmount = (float)((float)newHealth / (float)maxHealth);
        healthText.text = newHealth.ToString();
    }
    public void UpdateMana(int maxMana, int newMana)
    {
        manaFill.fillAmount = (float)((float)newMana / (float)maxMana);
        manaText.text = newMana.ToString();
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
[Serializable]
public class GoldUI
{
    [SerializeField] TextMeshProUGUI goldText;

    public void UpdateGold(int amount)
    {
       goldText.text = amount.ToString();
    }
}
[Serializable]
public class InventoryUI
{
    [SerializeField] List<UIItemSlot> slots = new List<UIItemSlot>();

    public void AssignItem(UIItemInfo itemInfo)
    {
        foreach(var slot in slots)
        {
            if (!slot.isFull)
            {
                slot.Assign(itemInfo);
                slot.isFull = true;
                break;
            }
        }
    }
    public bool IsInventoryFull()
    {
        bool isFull = true;
        foreach(var slot in slots)
        {
            if (!slot.isFull) isFull = false;
        }
        return isFull;
    }
}
