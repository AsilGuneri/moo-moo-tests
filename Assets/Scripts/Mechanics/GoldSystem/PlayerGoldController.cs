using Mirror;
using System;
using UnityEngine;

public class PlayerGoldController : NetworkBehaviour
{
    [SyncVar(hook = nameof(OnGoldChanged))]
    private int gold;

    private void OnGoldChanged(int oldGold, int newGold)
    {
        LocalPlayerUI.Instance.GoldUI.UpdateGold(newGold);
    }

    public int GetGold()
    {
        return gold;
    }

    [Command]
    public void CmdAddGold(int amount)
    {
        gold += amount;
    }

    [Command]
    public void CmdSpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
        }
        else
        {
            Debug.LogWarning("Not enough gold to spend!");
        }
    }
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