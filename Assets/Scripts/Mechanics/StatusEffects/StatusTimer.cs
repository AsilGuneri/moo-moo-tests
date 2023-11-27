using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusTimer : MonoBehaviour
{

    [SerializeField] private Image fillImage;

    private float totalTime = 0;
    private float timeRemaining = 0;
    private bool isRunning = false;

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

    public void StartTimer(float time, Color color = default)
    {
        if (color == default) color = Color.white;
        fillImage.color = color;
        timeRemaining = time;
        totalTime = time;
        isRunning = true;
    }
}