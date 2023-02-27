using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevelController : NetworkBehaviour
{
    [SyncVar] int currentExperience = 0;
    [SyncVar] int currentLevel = 1;
    int levelUpBaseCost = 1;

    [SerializeField] TextMeshProUGUI levelText;

    private void Start()
    {
        SetLevelText();
    }

    public void GainExperience(int exp)
    {
        currentExperience += exp;
        if (currentExperience >= ExperienceRequired())
        {
            LevelUp();
        }
    }
    private int ExperienceRequired()
    {
        return levelUpBaseCost * (currentLevel + 1);
    }
    private void LevelUp()
    {
        currentLevel++;
        currentExperience = 0;
        SetLevelText();
    }
    private void SetLevelText()
    {
        levelText.text = currentLevel.ToString();
    }
}
