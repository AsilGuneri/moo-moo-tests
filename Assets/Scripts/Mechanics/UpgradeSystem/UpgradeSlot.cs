using DuloGames.UI;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    public UpgradeData Data { get => data; }

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image upgradeIcon;
    [SerializeField] private Button selectButton;

    private UpgradeData data;

    private void Awake()
    {
        // toggle = GetComponent<Toggle>();
        selectButton.onClick.AddListener(OnUpgradeAcquired);
    }
    public void Setup(UpgradeData data)
    {
        this.data = data;
        nameText.text = data.DisplayName;
        descriptionText.text = data.Description;
        upgradeIcon.sprite = data.Icon;
    }
    void OnUpgradeAcquired()
    {
        //var manager = (CustomNetworkRoomManager)NetworkRoomManager.singleton;
        var x = NetworkClient.localPlayer.GetComponent<StatController>();
       // var statController = manager.GetLocalPlayer().StatController;
        data.OnAcquire(x,0);
        UpgradeManager.Instance.OnUpgradeAcquired(data);
    }


}
