using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Mirror;
using System;
using MyBox;

public class GoldManager : NetworkSingleton<GoldManager>
{
    
}

[Serializable]
public class PlayerStats
{
    public int TotalDamageDealt;
    public int TotalHealAmount;
    public int TotalDamageTanked;

    public void ResetStats()
    {
        TotalDamageDealt = 0;
        TotalHealAmount = 0;
        TotalDamageTanked = 0;
    }
}