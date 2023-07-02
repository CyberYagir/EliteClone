using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Galaxy;
using Core.Location;
using Core.PlayerScripts;
using Core.Systems;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Death
{
    public class DeathDataCollector : Singleton<DeathDataCollector>
    {
        public PlayerData playerData { get; private set; }
        public SavedSolarSystem savedSolarSystem;
        public SaveLoadData saves { get; private set; }
    
        public OrbitStation findNearStation { get; private set; }
        public SolarSystem findNear { get; private set; }

        public Event OnInited = new Event();

        private Cargo cargo;
        private FilesSystemHandler filesSystemHandler;
        private WorldDataHandler worldDataHandler;
        
        
        private void Awake()
        {
            Single(this);
            InitDataCollector();
        }

        public virtual void InitDataCollector()
        {
            
            worldDataHandler.DestroyShipPlayer();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            saves = GetComponent<SaveLoadData>();
            playerData = saves.LoadData();
        
            SystemLoad();
            CargoManage();

        
            OnInited.Invoke();
        }

        public void SystemLoad()
        {
            GalaxyGenerator.LoadSystems();
            savedSolarSystem = JsonConvert.DeserializeObject<SavedSolarSystem>(File.ReadAllText(PlayerDataManager.Instance.FSHandler.CurrentSystemFile));
            findNearStation = null;
        
            var solar = GalaxyGenerator.systems[savedSolarSystem.systemName.Split('.')[0]];
            PlayerDataManager.Instance.WorldHandler.ChangeSolarSystem(SolarStaticBuilder.Generate(solar));
            StartCoroutine(FindStation(solar));

        }
    
        public void CargoManage()
        {
            cargo = GetComponent<Cargo>();
            cargo.CustomInit(playerData, playerData.Ship.GetShip());


        }

        public IEnumerator FindStation(SolarSystem system)
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < system.stations.Count; i++)
            {
                if (findNearStation == null)
                {
                    findNear = system;
                    findNearStation = system.stations[i];
                }
                yield break;
            }

            for (int i = 0; i < system.sibligs.Count; i++)
            {
                StartCoroutine(FindStation(GalaxyGenerator.systems[system.sibligs[i].solarName]));
            }
        }


        public void Back()
        {
            PlayerDataManager.Instance.WorldHandler.ChangeSolarSystem(SolarStaticBuilder.Generate(findNear));
            LocationGenerator.SaveLocationFile(findNearStation.name, LocationPoint.LocationType.Station, new Dictionary<string, object>());
            if (!File.Exists(SolarStaticBuilder.GetSystemFileName()))
            {
                SolarStaticBuilder.SaveSystem();
            }
            SolarStaticBuilder.SystemLoad();

            var newShip = ItemsManager.GetShipItem(0).SaveShip();
            if (newShip.shipName == playerData.Ship.shipName)
            {
                for (int i = 0; i < playerData.Ship.slots.Count; i++)
                {
                    newShip.slots[i].button = playerData.Ship.slots[i].button;
                }
            }

            playerData.Ship = newShip;
            saves.SetKey("loc_start_on_pit", true, false);
            saves.SetKey("system_start_on", findNearStation.name, false);
            playerData.Keys = saves.GetKeys();

            var items = new List<Cargo.ItemData>();
            var credits = cargo.items.Find(x => x.id.idname == "credit");

            if (credits != null)
            {
                items.Add(new Cargo.ItemData { idName = credits.id.idname, value = credits.amount.value});
            }
        
            playerData.items = items;
            saves.SaveData(playerData);
            World.LoadLevel(Scenes.Garage);
        }
    }
}
