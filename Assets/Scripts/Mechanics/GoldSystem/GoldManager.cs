using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Mirror;
using System;
using MyBox;

public class GoldManager : NetworkSingleton<GoldManager>
{
    [SerializeField][ReadOnly] private Bank bank = new Bank();
    public Bank GameBank { get { return bank; } }

    private CustomNetworkRoomManager CustomManager;
    private void Awake()
    {
        CustomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }
    [ButtonMethod]
    public void DistributeGold(float totalGold)
    {

        float totalScore = CalculateTotalContributionScore(CustomManager.GamePlayers);

        foreach (PlayerMertController player in CustomManager.GamePlayers)
        {
            float percentage = CalculateContributionPercentage(player.Stats, totalScore);
            int goldReward = Mathf.CeilToInt(totalGold * percentage);
            GameBank.GiveGold(goldReward, player);
            player.Stats.ResetStats();
        }
    }
    public float CalculateContributionScore(PlayerStats stats)
    {
        float damageWeight = 1.0f;
        float healWeight = 0.8f;
        float tankWeight = 0.6f;

        return (stats.TotalDamageDealt * damageWeight) + (stats.TotalHealAmount * healWeight) + (stats.TotalDamageTanked * tankWeight);
    }
    private float CalculateTotalContributionScore(List<PlayerMertController> players)
    {
        float totalScore = 0;
        foreach (PlayerMertController player in players)
        {
            totalScore += CalculateContributionScore(player.Stats);
        }
        return totalScore;
    }
    public float CalculateContributionPercentage(PlayerStats stats, float totalScore)
    {
        float playerScore = CalculateContributionScore(stats);
        return playerScore / totalScore;
    }
}
[Serializable]
public class Bank
{
    public List<BankAccount> BankAccounts = new List<BankAccount>();

    public void AddBankAccount(PlayerMertController player)
    {
        BankAccount newAccount = new BankAccount();
        newAccount.PlayerController = player;
        newAccount.SetGold(0);
        BankAccounts.Add(newAccount);
    }
    public void GiveGold(int amount, PlayerMertController player)
    {
        var account = GetAccountHolder(player);
        account.AddGold(amount);
        Debug.Log($"{amount} golds given to player {player}");
    }
    public int GetGoldAmount(PlayerMertController player)
    {
        var account = GetAccountHolder(player);
        return account.Gold;
    }
    private BankAccount GetAccountHolder(PlayerMertController player)
    {
        foreach (BankAccount account in BankAccounts)
        {
            if (account.PlayerController == player) return account;
        }
        Debug.LogError("No account holder found by playerController");
        return null;
    }
}
[Serializable]
public class BankAccount
{
    [ReadOnly][SerializeField] private int gold;
    public int Gold
    {
        get
        {
            return gold;
        }
        private set
        {
            gold = value;
        }
    }
    public PlayerMertController PlayerController;

    public void SetGold(int newGoldAmount)
    {
        Gold = newGoldAmount;
        ClientGoldManager.Instance.UpdateGold(PlayerController.netIdentity.connectionToClient, gold);
    }
    public void AddGold(int additionAmount)
    {
        Gold += additionAmount;
        ClientGoldManager.Instance.UpdateGold(PlayerController.netIdentity.connectionToClient, gold);
    }
    public void PayGold(int price)
    {
        Gold -= price;//use out parameter here.
        ClientGoldManager.Instance.UpdateGold(PlayerController.netIdentity.connectionToClient, gold);
    }
}
[Serializable]
public class PlayerStats
{
    public float TotalDamageDealt;
    public float TotalHealAmount;
    public float TotalDamageTanked;

    public void ResetStats()
    {
        TotalDamageDealt = 0;
        TotalHealAmount = 0;
        TotalDamageTanked = 0;
    }
}