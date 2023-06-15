using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyIndicator : Indicator
{
    [SerializeField] private float destroyTime;

    public override void SetupIndicator()
    {
        Invoke(nameof(DestroyIndicator), destroyTime);
    }
    private void DestroyIndicator()
    {
        IndicatorManager.Instance.DestroyIndicator(gameObject);
    }
    
}
