using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;
using Mirror;

public class UnitManager : NetworkSingleton<UnitManager> 
{
    public readonly SyncList<NetworkIdentityReference> Players = new SyncList<NetworkIdentityReference>();
    public readonly SyncList<NetworkIdentityReference> WaveEnemies = new SyncList<NetworkIdentityReference>();

    [ServerCallback]
    public void RegisterUnit(NetworkIdentityReference unit, UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Player:
                if (Players.Contains(unit)) return;
                Players.Add(unit);
                break;
            case UnitType.WaveEnemy:
                if (WaveEnemies.Contains(unit)) return;
                WaveEnemies.Add(unit);
                break;

        }
    }
    [ServerCallback]
    public void UnregisterUnits(NetworkIdentityReference unit, UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Player:

                foreach(var player in Players)
                {
                    if (player.networkId == unit.networkId) { Players.Remove(player);}
                }
                break;
            case UnitType.WaveEnemy:
                foreach (var enemy in WaveEnemies)
                {
                    if (enemy.networkId == unit.networkId) 
                    { 
                        WaveEnemies.Remove(enemy);
                        if (WaveEnemies.Count <= 0) WaveManager.Instance.OnWaveEnd();
                    }
                }
                break;
        }
    }

    [ServerCallback]
    public void UnregisterUnits(uint netId, UnitType unitType)
    {
        switch (unitType)
        {
            case UnitType.Player:

                foreach(var player in Players)
                {
                    if (player.networkId == netId) { Players.Remove(player); Debug.Log("Player Removed From UnitList");}
                }
                break;
            case UnitType.WaveEnemy:
                foreach (var enemy in WaveEnemies)
                {
                    if (enemy.networkId == netId) 
                    { 
                        WaveEnemies.Remove(enemy);
                        if (WaveEnemies.Count <= 0) WaveManager.Instance.OnWaveEnd();
                    }
                }
                break;
        }
    }

    public GameObject GetClosestUnit(Vector3 myPosition, bool isEnemy = false)
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestUnit = null;
        foreach (NetworkIdentityReference unit in isEnemy ? WaveEnemies : Players)
        {
            if (!unit.Value.gameObject) continue;
            float distance = Vector3.Distance(myPosition, unit.Value.gameObject.transform.position);
            if (closestDistance < distance) continue;
            closestDistance = distance;
            closestUnit = unit.Value.gameObject;
        }
        return closestUnit;
    }

    public PlayerMertController GetPlayerController()
    {
        foreach(var player in Players)
        {
            if (player.Value.gameObject.GetComponent<PlayerMertController>().hasAuthority) return player.Value.gameObject.GetComponent<PlayerMertController>();
        }
        return null;
    }
}
public enum UnitType
{
    Player,
    WaveEnemy
}
