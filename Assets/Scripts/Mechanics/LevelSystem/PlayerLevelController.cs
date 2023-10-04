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


    public int CurrentLevel { get { return currentLevel; } }



    [Server]
    public void GainExp(int exp)
    {
        currentExperience += exp;
        if (currentExperience >= ExperienceRequired())
        {
            LevelUp();
        }
        //UIStatsManager.Instance.UpdateSlider(currentExperience, ExperienceRequired());
        GainExpVisual(currentExperience, ExperienceRequired());
    }
    [TargetRpc]
    private void GainExpVisual(float currentValue, float maxValue)
    {
        LocalPlayerUI.Instance.UpdateExpBar(currentValue, maxValue);
    }
    private int ExperienceRequired()
    {
        return levelUpBaseCost * (currentLevel + 1);
    }
    [Server]
    private void LevelUp()
    {
        currentLevel++;
        currentExperience = 0;
    }

}
