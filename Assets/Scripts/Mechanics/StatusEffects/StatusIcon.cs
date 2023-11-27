using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private Image[] icons;
    [SerializeField] private Image fillImage;

    private float totalTime = 0;
    private float timeRemaining = 0;
    private bool isRunning = false;

    public void Setup(Sprite icon, float time)
    {
        timeRemaining = time;
        totalTime = time;
        isRunning = true;
        foreach (var image in icons)
        {
            image.sprite = icon;
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            timeRemaining -= Time.deltaTime;
            fillImage.fillAmount = timeRemaining / totalTime;
            if (timeRemaining <= 0)
            {
                EndTimer();
            }
        }
    }
    private void EndTimer()
    {
        isRunning = false;
        timeRemaining = 0;
        totalTime = 0;
    }

    
}
