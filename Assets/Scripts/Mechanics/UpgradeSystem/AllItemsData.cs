using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AllItemsData", menuName = "Scriptable Objects/Managers/AllItemsData")]
public class AllItemsData : ScriptableSingleton<AllItemsData>
{
    public List<UpgradeData> AllItems;


}
