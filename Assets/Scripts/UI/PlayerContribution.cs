using UnityEngine;
using System;
using TMPro;
using Mirror;

public class PlayerContribution : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerNameText;
    [SerializeField] TextMeshProUGUI ContributionPercentageText;
    public PlayerMertController PlayerController { get; set; }

    [ClientRpc]
    public void UpdatePlayerNameText(string text)
    {
        PlayerNameText.text = text;
    }
    [ClientRpc]
    public void UpdateContributionPercentText(float amount)
    {
        var text = (amount * 100).ToString("F1");
        ContributionPercentageText.text = text;
    }

}
