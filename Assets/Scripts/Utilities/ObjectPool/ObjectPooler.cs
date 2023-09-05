using UnityEngine;
using Mirror;
using System.Collections.Generic;
using System;
using System.Xml.Linq;

public class ObjectPooler : NetworkBehaviour
{
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    
}