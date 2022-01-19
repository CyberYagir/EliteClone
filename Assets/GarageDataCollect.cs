using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GarageDataCollect : MonoBehaviour
{
    public static GarageDataCollect Instance;
    [HideInInspector]
    public PlayerData playerData;
    [HideInInspector]
    public Location playerLocation;
    public int stationSeed;
    private void Awake()
    {
        Instance = this;
    }

    public void InitDataCollector()
    {
        playerData = GetComponent<SaveLoadData>().LoadData();
        playerLocation = JsonConvert.DeserializeObject<Location>(File.ReadAllText(PlayerDataManager.CurrentLocationFile));
        stationSeed = WorldOrbitalStation.CalcSeed(playerLocation.locationName);
    }
}
