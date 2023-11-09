using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameStatSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName;

    public void Setup(string playerName)
    {
        this.playerName.text = playerName;
    }
}
