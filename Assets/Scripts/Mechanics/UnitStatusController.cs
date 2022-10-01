using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitStatusController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Health health;

    private float timer = 0;
    private float intervalCounter = 0;

    private bool isBurning = false;
    private int _burnDmg;
    private float _burnTime;
    private float _burnInterval;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<Health>();
    }
    private void FixedUpdate()
    {
        if (isBurning)
        {
            Burn();
        }
    }

    private void Burn()
    {
        timer += Time.deltaTime;
        intervalCounter += Time.deltaTime;
        if (intervalCounter > _burnInterval)
        {
            health.TakeDamage(_burnDmg);
            intervalCounter = 0;
        }
        if (timer > _burnTime) isBurning = false;
    }

    public void SetBurn(int burnDmg, float second, float timeInterval)
    {
        isBurning = true;
        _burnDmg = burnDmg;
        _burnTime = second;
        _burnInterval = timeInterval;
    }
}

