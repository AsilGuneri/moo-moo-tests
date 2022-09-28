using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataHolder : MonoBehaviour
{
    [SerializeField] private InputKeysData keysData;
    [SerializeField] private HeroBaseStatsData heroStatsData;

    public HeroBaseStatsData HeroStatsData { get { return heroStatsData; } }
    public InputKeysData KeysData { get { return keysData; } }
}
