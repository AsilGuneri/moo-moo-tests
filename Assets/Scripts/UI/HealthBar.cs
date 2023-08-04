using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText; 

    private int maxHp = 0;

    public void SetupHealthBar(int maxHp, bool updateRequired = false, int newHp = 0)
    {
        healthSlider.value = 1;
        healthText.text = maxHp.ToString();
        this.maxHp = maxHp;
        if (updateRequired)
        {
            UpdateHealthBar(newHp);
        }
    }

    public void UpdateHealthBar(int newHp)
    {
        if (this.healthSlider != null)
        {
            healthSlider.value = (float)((float)newHp / (float)maxHp);
            healthText.text = newHp.ToString();
        }
    }

}
