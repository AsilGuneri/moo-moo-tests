using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Image bar;
    [SerializeField] TextMeshProUGUI stackText;

    float totalTime;
    Status currentStatus;

    public void OnUpdate()
    {
        bar.fillAmount = currentStatus.Time / totalTime;
    }

    public void DestroyUI()
    {
        Destroy(gameObject);
    }

    public void Setup(Status status, float time)
    {
        icon.sprite = status.Data.IconSprite;
        bar.color = status.Data.TimerColor;
        totalTime = time;
        currentStatus = status;
    }
    public void UpdateStatusCount(Status newStatus, int count)
    {
        stackText.text = count.ToString();
        currentStatus = newStatus;
    }
}