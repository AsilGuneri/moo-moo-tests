using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using MyBox;

public class Indicators : MonoBehaviour
{
    [SerializeField] private GameObject moveIndicator;
    [SerializeField] private GameObject attackIndicator;

    private GameObject _currentIndicator;

    public void Setup(Vector3 pos,bool isMove)
    {
        _currentIndicator = isMove ? moveIndicator : attackIndicator;

        if (transform.parent != null) transform.parent = null;
        transform.position = new Vector3(pos.x, pos.y, pos.z);
       _currentIndicator.GetComponent<ParticleSystem>().Play();
    }

}

