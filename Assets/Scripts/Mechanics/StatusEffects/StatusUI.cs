using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] private Image bar;

    [SerializeField] Transform iconRoot;
    [SerializeField] private Transform barRoot;

    float remainingTime;
    float totalTime;

    public void OnUpdate(float remainingTime)
    {
        this.remainingTime = remainingTime;
        bar.fillAmount = remainingTime / totalTime;
    }

    public void DestroyUI()
    {
        Destroy(iconRoot.gameObject);
        Destroy(barRoot.gameObject);
        Destroy(gameObject);
    }

    public void Setup(StatusData data, float time, Transform iconParent, Transform barParent)
    {
        icon.sprite = data.IconSprite;
        bar.color = data.TimerColor;
        remainingTime = time;
        totalTime = time;
        iconRoot.SetParent(iconParent, false);
        barRoot.SetParent(barParent, false);
    }
}