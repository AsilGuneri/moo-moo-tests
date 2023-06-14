using UnityEngine;
using System;
using TMPro;
using Mirror;

public class PlayerContribution : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI PlayerNameText;
    [SerializeField] TextMeshProUGUI ContributionPercentageText;
    public PlayerController PlayerController { get; set; }

    private void OnEnable()
    {
        transform.SetParent(ContributionPanel.Instance.ContributionFieldParent, false);
    }

    [ClientRpc]
    public void RpcUpdatePlayerNameText(string text)
    {
        PlayerNameText.text = text;
    }
    [ClientRpc]
    public void RpcUpdateContributionPercentText(float amount)
    {
        var text = (amount * 100).ToString("F1");
        ContributionPercentageText.text = text;
    }

}
