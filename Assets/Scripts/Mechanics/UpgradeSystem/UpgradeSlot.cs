using DuloGames.UI;
using Mirror;
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

    private UpgradeData data;
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnSlotClicked);
        
    }
    public void Setup(UpgradeData data)
    {
        this.data = data;
        nameText.text = data.DisplayName;
        descriptionText.text = data.Description;
        upgradeIcon.sprite = data.Icon;
    }
    private void OnSlotClicked(bool isOn)
    {
        UpgradeManager.Instance.OnSlotClicked(this, isOn);
    }


}
