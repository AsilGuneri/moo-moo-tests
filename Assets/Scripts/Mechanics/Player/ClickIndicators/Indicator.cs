using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Indicator : MonoBehaviour
{
    public bool IsLocal { get => isLocal; }
    [SerializeField] protected bool isLocal;

    public abstract void SetupIndicator();
}