using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText; 


    public void UpdateHealthBar(int maxHp, int newHp)
    {
        healthSlider.value = (float)((float)newHp / (float)maxHp);
        healthText.text = newHp.ToString();
        LocalPlayerUI.Instance.UpdateHealthBar(maxHp, newHp);
    }

}
