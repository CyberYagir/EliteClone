using System.IO;
using Core.Game;
using Core.Location;
using Core.PlayerScripts;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Garage
{
    public class GarageDataCollect : MonoBehaviour
    {
        public static GarageDataCollect Instance;
        public PlayerData playerData { get; private set; }
        public Cargo cargo  { get; private set; }
        public Location.Location playerLocation  { get; private set; }
    
        public ItemShip ship { get; private set; }

        public SaveLoadData saves { get; private set; }
    
        public static Event OnChangeShip = new Event();
    
        public int stationSeed;
        private void Awake()
        {
            Instance = this;
        }

        public virtual void InitDataCollector()
        {
            saves = GetComponent<SaveLoadData>();
            playerData = saves.LoadData();
            if (File.Exists(PlayerDataManager.CurrentLocationFile))
            {
                playerLocation = JsonConvert.DeserializeObject<Location.Location>(File.ReadAllText(PlayerDataManager.CurrentLocationFile));
            }
            else
            {
                World.LoadLevel(Scenes.Location);
                return;
            }

            stationSeed = WorldOrbitalStation.CalcSeed(playerLocation.locationName, playerLocation.GetSystemCode());
            cargo = GetComponent<Cargo>();
            ChangeShip(playerData.Ship.GetShip());
            cargo.CustomInit(playerData, ship);
        }


        public void ChangeShip(ItemShip newShip)
        {
            ship = newShip;
            cargo.SetShip(newShip);
            OnChangeShip.Invoke();
        }

        public virtual void Save()
        {
            print("Save Garage");
            playerData.Ship = ship.SaveShip();
            playerData.shipsInStations = saves.GetStorageShip();
            playerData.items = cargo.GetData();
            playerData.playedTime = saves.GetTime();
            saves.SaveData(playerData);
        }
    }
}
