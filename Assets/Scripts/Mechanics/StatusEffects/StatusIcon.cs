using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    public void ChangeIconSprite(Sprite icon)
    {
        iconImage.sprite = icon;
    }
}
