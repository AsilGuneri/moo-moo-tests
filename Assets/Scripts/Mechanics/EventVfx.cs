using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventVfx : MonoBehaviour
{
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }
    private void Start()
    {
        controller.AttackController.OnStartAttack += x;
    }
    private void x()
    {

    }
}
