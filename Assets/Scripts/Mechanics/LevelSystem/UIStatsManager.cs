using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatsManager : Singleton<UIStatsManager>
{
    [SerializeField] Slider expSlider;
    [SerializeField] TextMeshProUGUI levelText;

    private void Start()
    {
        UpdateLevelText("1");
    }
    public void UpdateSlider(float currentValue, float maxValue)
    {
        // Calculate the slider value as a percentage of the max value
        float sliderValue = 100f * (currentValue / maxValue);

        // Set the value of the expSlider slider
        expSlider.value = sliderValue;
    }
    public void UpdateLevelText(string level)
    {
        levelText.text = level;
    }

}
