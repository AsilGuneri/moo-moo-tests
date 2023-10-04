using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class LocalPlayerUI : NetworkSingleton<LocalPlayerUI>
{
    [SerializeField] Image healthFill;
    [SerializeField] TextMeshProUGUI healthText;

    [SerializeField] Image expFill;


    public void UpdateHealthBar(int maxHp, int newHp)
    {
        //if (!isOwned) return;
        healthFill.fillAmount = (float)((float)newHp / (float)maxHp);
        healthText.text = newHp.ToString();
    }
    public void UpdateExpBar(float currentValue, float maxValue)
    {
        float fillAmount = currentValue / maxValue;
        expFill.fillAmount= fillAmount;
    }
}
