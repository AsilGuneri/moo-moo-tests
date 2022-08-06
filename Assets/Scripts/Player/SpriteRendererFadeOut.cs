using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererFadeOut : MonoBehaviour
{
    [SerializeField] private float time;

    public void Setup(Vector3 pos)
    {
        if (transform.parent != null) transform.parent = null;
        var sprite = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(pos.x, transform.position.y, pos.z);
        gameObject.SetActive(true);
        sprite.DOFade(1, 0);
        sprite.DOFade(0, time).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
