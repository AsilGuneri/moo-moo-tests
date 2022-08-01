using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using System;
using Mirror;

public class WaveManager : Singleton<WaveManager>//
{
    public Wave[] waves;
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private int[] newSkillWaves;

    private int _currentWave = 0;
    private SkillTier _currentTier;

    public SkillTier CurrentTier
    {
        get { return _currentTier; }
        set { _currentTier = value; }
    }

    public Action OnWaveEnd;

    private void Start()
    {
        OnWaveEnd += EndWave;
    }
    public void SpawnWave(Wave nextWave)
    {
        float minionPerLine = Mathf.Sqrt(nextWave.minionPerPoint);
        
        foreach (SpawnPoint point in spawnPoints)
        {
            //setup point, point e vector3 listesi a� i�ini doldur spawn spaceleri ka�ele, spawn spaceler de�i�medik�e tekrar setup � �a��rma
            float spawnSpaceZ = point.isHorizontal ? nextWave.spawnSpace.x : nextWave.spawnSpace.z;
            float spawnSpaceX = point.isHorizontal ? nextWave.spawnSpace.z : nextWave.spawnSpace.x;

                for (int j = 0; j < minionPerLine; j++)
                {
                    float spawnZPos = (point.transform.position.z - ((minionPerLine -1) * spawnSpaceZ / 2)) + (j * nextWave.spawnSpace.z);
                    for (int k = 0; k < minionPerLine; k++)
                    {
                        float spawnXPos = (point.transform.position.x - ((minionPerLine - 1) * spawnSpaceX / 2)) + (k * nextWave.spawnSpace.x);
                        Vector3 spawnPoint = new Vector3(spawnXPos, nextWave.spawnSpace.y, spawnZPos);
                    //ObjectPooler.Instance.Spawn(nextWave.prefab.name, spawnPoint, Quaternion.identity); ///TODO: Use pool
                    var obj = Instantiate(nextWave.prefab, spawnPoint, Quaternion.identity);
                    obj.GetComponent<BasicEnemyController>().Activate();
                    NetworkServer.Spawn(obj);
                    UnitManager.Instance.RegisterUnit(new NetworkIdentityReference(obj.GetComponent<NetworkIdentity>()), UnitType.WaveEnemy);
                    }

                }
            
        }
    }
    private void EndWave()
    {
        if (IsNewSkillWave())
        {
            List<Skill> skillsToSet = new List<Skill>();
            PlayerMertController playerController = UnitManager.Instance.GetPlayerController();
            PlayerSkillController playerSkillController = playerController.GetComponent<PlayerSkillController>();
            List<Skill> possibleSkills = SkillManager.Instance.GetSkillGroup(playerController.ClassType, playerController.CurrentTier);


            foreach(var skill in possibleSkills)
            {
                skillsToSet.Add(skill);
            }
        } //open skill select ui
        else SpawnNextWave();
    }
    private void SpawnNextWave()
    {
        _currentWave++;
        SpawnWave(waves[_currentWave]);
    }
    private bool IsNewSkillWave()
    {
        for (int i = 0; i < newSkillWaves.Length; i++)
        {
            if(_currentWave +1 == newSkillWaves[i])
            {
                return true;
            }
        }
        return false;
    }
}
[Serializable]
public class Wave
{
    public int waveIndex;
    public GameObject prefab;
    public int minionPerPoint;
    public Vector3 spawnSpace;
    public bool isHorizontal;
    
}

