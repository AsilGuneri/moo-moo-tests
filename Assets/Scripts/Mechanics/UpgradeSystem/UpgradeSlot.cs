using DuloGames.UI;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI nameText;

    public void Setup()
    {
        nameText.text = "";
    }


}
