using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Utilities;
using Mirror;
using UnityEngine.UIElements;

public class IndicatorManager : NetworkSingleton<IndicatorManager>
{
    public List<Indicator> Indicators = new List<Indicator>();

    public void StartIndicator(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var indicator = GetIndicator(prefab);
        GameObject indicatorObj = SpawnIndicatorLocally(indicator.gameObject, position, rotation);
        if (!indicator.IsLocal)
        {
            CmdSpawnIndicator(indicatorObj);
        }
    }
    public void DestroyIndicator(GameObject indicator)
    {
        if(!indicator.GetComponent<Indicator>().IsLocal) 
        {
            CmdUnspawnIndicator(indicator);
        }
        ObjectPooler.Instance.Return(indicator.gameObject);
    }

    private Indicator GetIndicator(GameObject prefab)
    {
        foreach(Indicator indicator in Indicators)
        {
            if(indicator.gameObject == prefab) return indicator;
        }
        Debug.Log($"Indicator returned null");
        return null;
    }
    [Command(requiresAuthority = false)]
    private void CmdSpawnIndicator(GameObject objToSpawn)
    {
        NetworkServer.Spawn(objToSpawn);
    }
    [Command(requiresAuthority = false)]
    private void CmdUnspawnIndicator(GameObject indicator)
    {
        ObjectPooler.Instance.Return(indicator.gameObject);
    }
    private GameObject SpawnIndicatorLocally(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = ObjectPooler.Instance.Get(prefab, position, rotation);
        obj.GetComponent<Indicator>().SetupIndicator();
        return obj;
    }
  

}
