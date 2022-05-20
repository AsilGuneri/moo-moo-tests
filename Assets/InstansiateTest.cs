using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstansiateTest : MonoBehaviour
{
    public GameObject prefab;
    public Transform pos;
    void Start()
    {
        InvokeRepeating(nameof(Test), 1f, 10f);
    }

    private void Test()
    {
        Instantiate(prefab, pos);
    }
}
