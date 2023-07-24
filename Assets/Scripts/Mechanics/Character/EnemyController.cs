using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : UnitController
{
    public MinionType MinionType { get => minionType; }
    [SerializeField] private MinionType minionType;
    protected override void Start()
    {
        base.Start();
        SubscribeAnimEvents();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)&& minionType == MinionType.Commander)
        {
            foreach (var enemy in UnitManager.Instance.WaveEnemies)
            {
                if (enemy.Value.TryGetComponent(out EnemyController enemyController))
                {
                    Debug.Log("asilxx1 " + name);
                    if(enemyController.MinionType != MinionType.Commander)
                    {
                        Debug.Log("asilxx2 " + name);
                        enemy.Value.GetComponent<EnemyBrain>().SetPackRoutine("DefenceCall");
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            foreach (var enemy in UnitManager.Instance.WaveEnemies)
            {
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (var enemy in UnitManager.Instance.WaveEnemies)
            {
                enemy.Value.GetComponent<EnemyBrain>().SetPackRoutine("Default");
            }
        }
    }
}
