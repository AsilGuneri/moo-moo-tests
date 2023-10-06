using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Client]
    public void UpdateHealthBar(int maxHp, int newHp)
    {
        healthSlider.value = (float)((float)newHp / (float)maxHp);
        healthText.text = newHp.ToString();
        if (isLocalPlayer && isOwned)
        {
            LocalPlayerUI.Instance.HealthBarUI.UpdateHealthBar(maxHp, newHp);
        }
    }

}
