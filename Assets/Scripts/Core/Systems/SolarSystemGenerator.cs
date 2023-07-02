using System.IO;
using Core.Galaxy;
using Core.Location;
using Core.PlayerScripts;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Systems
{
    public class SolarSystemGenerator : MonoBehaviour
    {
        public GameObject sunPrefab, planetPrefab, stationPointPrefab, player, systemPoint, beltPoint;
        private WorldDataHandler worldHandler;
        private FilesSystemHandler filesSystemHandler;


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
                if (FindObjectOfType<Player>() == null)
                {
                    Instantiate(player.gameObject).GetComponent<Player>().Init();
                }

                var playerInst = Player.inst.transform;
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

        private void Awake()
        {
            World.SetScene(Scenes.System);
            
            this.filesSystemHandler = PlayerDataManager.Instance.FSHandler;
            this.worldHandler = PlayerDataManager.Instance.WorldHandler;
            
        }

        private void OnEnable()
        {
            SolarStaticBuilder.ClearSavedSystem();
            
            InitSystem();
            CreateSystem();
            GetComponent<SolarSystemShips>().Init();
        }

        private void Update()
        {
            if (Player.inst.saves.ExKey("system_start_on"))
            {
                Player.inst.transform.position = GameObject.Find((string) Player.inst.saves.GetKeys()["system_start_on"]).transform.position;
                Player.inst.saves.DelKey("system_start_on");
            }

            enabled = false;
        }


        private void OnApplicationQuit()
        {
            SolarStaticBuilder.SaveSystem();
        }
    }
}