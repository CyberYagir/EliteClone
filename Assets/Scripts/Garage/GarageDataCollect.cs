using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Game;
using Newtonsoft.Json;
using UnityEngine;

public class GarageDataCollect : MonoBehaviour
{
    public static GarageDataCollect Instance;
    public PlayerData playerData { get; private set; }
    public Cargo cargo  { get; private set; }
    public Location playerLocation  { get; private set; }
    
    public ItemShip ship { get; private set; }
    
    
    [SerializeField]
    private ItemShip serializeShip;
    
    public static Event OnChangeShip = new Event();
    
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
        cargo = GetComponent<Cargo>();
        ship = playerData.Ship.GetShip();
        cargo.CustomInit(playerData, ship);
        serializeShip = ship;
        OnChangeShip.Invoke();
    }
}
