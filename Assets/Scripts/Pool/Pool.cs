using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Pool")]
public class Pool : ScriptableObject
{
    public string PoolName;
    public GameObject Prefab;
    public int StartingQuantity;
}
