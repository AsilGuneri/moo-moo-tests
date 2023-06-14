using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Utilities;

public class ContributionPanel : NetworkSingleton<ContributionPanel>
{
    public Transform ContributionFieldParent { get => contributionFieldParent; }
    [SerializeField] private PlayerContribution contributionFieldPrefab;
    [SerializeField] private Transform contributionFieldParent;
    private List<PlayerContribution> Contributions = new List<PlayerContribution>();

    private CustomNetworkRoomManager CustomManager;
    private void Awake()
    {
        CustomManager = NetworkRoomManager.singleton as CustomNetworkRoomManager;
    }

    [ServerCallback]
    public void AddPlayerContributionField(PlayerController playerController)
    {
        var field = Instantiate(contributionFieldPrefab);
        field.PlayerController = playerController;
        NetworkServer.Spawn(field.gameObject);
        field.RpcUpdatePlayerNameText(playerController.PlayerName);//temp
        Contributions.Add(field);
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateContribution()
    {
        UpdateAllContributions();
    }
    private void UpdateAllContributions()
    {
        //foreach (PlayerMertController player in CustomManager.GamePlayers)
        //{
        //    var contributionField = GetPlayersContributionField(player);
        //    float totalScore = CalculateTotalContributionScore(CustomManager.GamePlayers);
        //    float newPercentage = CalculateContributionPercentage(contributionField.PlayerController.Stats, totalScore);
        //    contributionField.RpcUpdateContributionPercentText(newPercentage);
        //}
    }
    private PlayerContribution GetPlayersContributionField(PlayerController playerController)
    {
        foreach(var contribution in Contributions)
        {
            if(playerController == contribution.PlayerController)
            {
                return contribution;
            }
        }
        Debug.LogError("Could not found contribution field");
        return null;
    }
    public float CalculateContributionScore(PlayerStats stats)
    {
        float damageWeight = 1.0f;
        float healWeight = 0f;
        float tankWeight = 0f;

        return (stats.TotalDamageDealt * damageWeight) + (stats.TotalHealAmount * healWeight) + (stats.TotalDamageTanked * tankWeight);
    }
    public float CalculateTotalContributionScore(List<PlayerController> players)
    {
        float totalScore = 0;
        foreach (PlayerController player in players)
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
