using System.IO;
using Core.Galaxy;
using Core.Location;
using Core.PlayerScripts;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Systems
{
    public class SolarSystemGenerator : StartupObject
    {
        [SerializeField] private GameObject sunPrefab;
        [SerializeField] private GameObject planetPrefab;
        [SerializeField] private GameObject stationPointPrefab;
        [SerializeField] private GameObject systemPoint;
        [SerializeField] private GameObject beltPoint;
        
        
        private WorldDataHandler worldHandler;
        private FilesSystemHandler filesSystemHandler;


        public override void Init(PlayerDataManager playerDataManager)
        {
            base.Init(playerDataManager);
            
            World.SetScene(Scenes.System);
            this.filesSystemHandler = playerDataManager.FSHandler;
            this.worldHandler = playerDataManager.WorldHandler;
            
            
            SolarStaticBuilder.ClearSavedSystem();
            InitSystem();
            CreateSystem();
            
            worldHandler.SetLocation(this);
        }

        public override void Loop()
        {
            var ship = worldHandler.ShipPlayer;
            if (ship.SaveData.ExKey("system_start_on"))
            {
               ship.transform.position = GameObject.Find((string) ship.SaveData.GetKeys()["system_start_on"]).transform.position;
               ship.SaveData.DelKey("system_start_on");
            }

            enabled = false;
        }


        public void InitSystem()
        {
            if (worldHandler.CurrentSolarSystem != null) //Generate And Save
            {
                GenerateAndSave();
            }
            else
            {
                bool notHaveErrors = true;
                if (File.Exists(filesSystemHandler.CurrentSystemFile))
                {
                    SolarStaticBuilder.SystemLoad();
                    var file = filesSystemHandler.CacheSystemsFolder + "/" + SolarStaticBuilder.SavedSolarSystem.systemName + ".solar";
                    if (File.Exists(file))
                    {
                        worldHandler.ChangeSolarSystem
                        (
                            JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(file))
                        );
                        notHaveErrors = false;
                    }
                }
                
                if (notHaveErrors) 
                    World.LoadLevel(Scenes.Init);
            }
        }

        private void GenerateAndSave()
        {
            if (!File.Exists(SolarStaticBuilder.GetSystemFileName()))
            {
                worldHandler.ChangeSolarSystem(
                    SolarStaticBuilder.Generate(worldHandler.CurrentSolarSystem)
                );
            }
            else
            {
                if (File.Exists(SolarStaticBuilder.GetSystemFileName()))
                    worldHandler.ChangeSolarSystem(
                        JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(SolarStaticBuilder.GetSystemFileName()))
                    );
            }
        }

        public void CreateSystem()
        {
            if (worldHandler.CurrentSolarSystem != null)
            {
                var playerInst = worldHandler.TryCreatePlayerShip().transform;
                playerInst.parent = transform;
                playerInst.parent = null;
                playerInst.position = Vector3.zero;
                SolarStaticBuilder.DrawAll(worldHandler.CurrentSolarSystem, transform, sunPrefab, planetPrefab, stationPointPrefab, systemPoint, beltPoint, SolarStaticBuilder.GalaxyScale, SolarStaticBuilder.SavedSolarSystem == null);
                if (SolarStaticBuilder.SavedSolarSystem != null)
                {
                    playerInst.position = SolarStaticBuilder.SavedSolarSystem.playerPos;
                    transform.position = SolarStaticBuilder.SavedSolarSystem.worldPos;
                }

                SolarStaticBuilder.SaveSystem();
            }
        }
        
        private void OnApplicationQuit()
        {
            SolarStaticBuilder.SaveSystem();
        }
    }
}