using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : NetworkBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private Image manaFill;

    [Client]
    public void UpdateHealthBar(int maxHealth, int newHealth)
    {

        if (healthFill) healthFill.fillAmount = (float)((float)newHealth / (float)maxHealth);
        if (isLocalPlayer && isOwned)
        {
            LocalPlayerUI.Instance.HealthBarUI.UpdateHealthBar(maxHealth, newHealth);
        }
    }
    [Client]
    public void UpdateMana(int maxMana, int newMana)
    {
        if(manaFill) manaFill.fillAmount = (float)((float)newMana / (float)maxMana);
        if (isLocalPlayer && isOwned)
        {
            LocalPlayerUI.Instance.HealthBarUI.UpdateMana(maxMana, newMana);
        }
    }

}
