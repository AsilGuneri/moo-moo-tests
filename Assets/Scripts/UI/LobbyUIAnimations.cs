using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUIAnimations : MonoBehaviour
{
    public static LobbyUIAnimations Instance { get; private set; }

    [SerializeField] private Transform background;
    [SerializeField] private float backgroundAnimTime;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        AnimateBackground();
    }
    private void AnimateBackground()
    {
        if (!background) return;
        background.DOScale(2, backgroundAnimTime).SetEase(Ease.InOutCubic);
    }
}
