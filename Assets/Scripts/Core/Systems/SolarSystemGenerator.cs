using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Core.Inject.FoldersManagerService;
using Core.Core.Inject.GlobalDataService;
using Core.Galaxy;
using Core.Game;
using Core.Location;
using Core.PlayerScripts;
using Newtonsoft.Json;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Core.Systems
{

    public partial class SolarSystemGenerator : MonoBehaviour
    {
        public GameObject sunPrefab, planetPrefab, stationPointPrefab, player, systemPoint, beltPoint;
        
        
        private SolarSystemService solarSystemService;
        private FolderManagerService folderManagerService;


        [Inject]
        public void Constructor(
            SolarSystemService solarSystemService, 
            FolderManagerService folderManagerService)
        {
            this.folderManagerService = folderManagerService;
            this.solarSystemService = solarSystemService;
        }
        
        public void InitSystem()
        {
            if (solarSystemService.CurrentSolarSystem != null) //Generate And Save
            {
                if (!File.Exists(GetSystemFileName(solarSystemService, folderManagerService)))
                {
                    solarSystemService.SetSolarSystem(Generate(solarSystemService.CurrentSolarSystem));
                }
                else
                {
                    if (File.Exists(GetSystemFileName(solarSystemService, folderManagerService)))
                        solarSystemService.SetSolarSystem(JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(GetSystemFileName())));
                }
            }
            else
            {
                if (File.Exists(folderManagerService.CurrentSystemFile))
                {
                    Load(folderManagerService);
                    
                    var file = folderManagerService.CacheSystemsFolder + "/" + savedSolarSystem.systemName + ".solar";
                    if (File.Exists(file))
                    {
                        solarSystemService.SetSolarSystem(JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(file)));
                    }
                    else
                    {
                        World.LoadLevel(Scenes.Init);
                    }
                }
                else
                {                   
                    World.LoadLevel(Scenes.Init);
                }
            }
        }

        public void CreateSystem()
        {
            if (solarSystemService.CurrentSolarSystem != null)
            {
                if (FindObjectOfType<Player>() == null)
                {
                    Instantiate(player.gameObject).GetComponent<Player>().Init();
                }

                var playerInst = Player.inst.transform;
                playerInst.parent = transform;
                playerInst.parent = null;
                playerInst.position = Vector3.zero; 
                DrawAll(solarSystemService, solarSystemService.CurrentSolarSystem, transform, sunPrefab, planetPrefab, stationPointPrefab, systemPoint, beltPoint, scale, savedSolarSystem == null);
                if (savedSolarSystem != null)
                {
                    playerInst.position = savedSolarSystem.playerPos;
                    transform.position = savedSolarSystem.worldPos;
                }

                SaveSystem(solarSystemService, folderManagerService);
            }
        }

        private void Awake()
        {
            World.SetScene(Scenes.System);
        }

        private void OnEnable()
        {
            savedSolarSystem = null;
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
            SaveSystem(solarSystemService, folderManagerService);
        }
    }
}