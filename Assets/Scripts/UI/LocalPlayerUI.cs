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


    public void UpdateHealthBar(int maxHp, int newHp)
    {
        //if (!isOwned) return;
        healthFill.fillAmount = (float)((float)newHp / (float)maxHp);
        healthText.text = newHp.ToString();
    }
}
