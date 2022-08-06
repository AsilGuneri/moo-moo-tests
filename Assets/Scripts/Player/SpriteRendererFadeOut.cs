using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MyBox;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererFadeOut : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private IndicatorGroup moveIndicator;
    [SerializeField] private IndicatorGroup attackIndicator;

    private Tween fadeReset;
    private Tween fadeOut;


    public void Setup(Vector3 pos,bool isMove)
    {
        var spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.sprite = isMove ? moveIndicator.IndicatorSprite : attackIndicator.IndicatorSprite;
        spriteRend.color = isMove ? moveIndicator.IndicatorColor : attackIndicator.IndicatorColor;

        fadeReset.Kill();
        fadeOut.Kill();
        if (transform.parent != null) transform.parent = null;
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        fadeReset =  spriteRend.DOFade(1, 0);
        fadeOut = spriteRend.DOFade(0, time);
    }

}
[System.Serializable]
public class IndicatorGroup
{
    public Sprite IndicatorSprite;
    public Color IndicatorColor;
}
