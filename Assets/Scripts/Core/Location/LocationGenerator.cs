using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Galaxy;
using Core.PlayerScripts;
using Core.Systems;
using Core.Systems.InteractivePoints;
using Newtonsoft.Json;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Location
{
    [Serializable]
    public class Location
    {
        public string systemName;
        public string locationName;
        public LocationPoint.LocationType type;
        public Dictionary<string, object> otherKeys = new Dictionary<string, object>();
        public string GetSystemCode()
        {
            var strings = systemName.Split(' ');
            if (strings.Length == 2)
            {
                return strings[1];
            }

            return systemName;
        }
    }
    public class LocationGenerator : StartupObject
    {
        [System.Serializable]
        public class SpawnedLocation
        {
            
            public enum SpawnedLocationType
            {
                OrbitalStation,
                MeteorBelt,
                BotsLocation
            }
            [SerializeField] private GameObject spawnedLocation;

            [SerializeField] private BeltGenerator beltGenerator;
            [SerializeField] private WorldOrbitalStation orbitStation;
            [SerializeField] private LocationBotPoint botPoint;


            private SingleInitialization singleInitialization;
            private SpawnedLocationType type;

            public SpawnedLocation(GameObject spawnedLocation, SpawnedLocationType type)
            {
                this.spawnedLocation = spawnedLocation;
                this.type = type;

                beltGenerator = spawnedLocation.GetComponent<BeltGenerator>();
                orbitStation = spawnedLocation.GetComponent<WorldOrbitalStation>();
                botPoint = spawnedLocation.GetComponent<LocationBotPoint>();

                singleInitialization = spawnedLocation.GetComponent<SingleInitialization>();
            }

            public SpawnedLocationType Type => type;
            public LocationBotPoint BotPoint => botPoint;
            public WorldOrbitalStation OrbitStation => orbitStation;
            public BeltGenerator BeltGenerator => beltGenerator;
            public GameObject Spawned => spawnedLocation;
        }
        
        
        public static Location CurrentSave;

        [SerializeField] private GameObject planet;
        [SerializeField] private GameObject sunPrefab;
        [SerializeField] private GameObject station;
        [SerializeField] private GameObject systemPoint;
        [SerializeField] private GameObject beltPoint;

        [SerializeField] private SpawnedLocation currentLocationData;

        public Event OnSetSystemToLocation = new Event();


        private WorldDataHandler worldHandler;
        private FilesSystemHandler filesSystemHandler;

        public SpawnedLocation CurrentLocationData => currentLocationData;

        public override void Init(PlayerDataManager playerDataManager)
        {
            base.Init(playerDataManager);
            
            
            worldHandler = playerDataManager.WorldHandler;
            filesSystemHandler = playerDataManager.FSHandler;
            
            World.SetScene(Scenes.Location);

            
            worldHandler.TryCreatePlayerShip().Init();


            InitLocation(playerDataManager);

            
            
            playerDataManager.WorldHandler.SetLocation(this);
        }
        
        private void InitLocation(PlayerDataManager playerDataManager)
        {
            CurrentSave = null;
            
            if (!File.Exists(PlayerDataManager.Instance.FSHandler.CurrentLocationFile))
            {
                World.LoadLevel(Scenes.System);
                return;
            }


            LoadLocation();
            SetSystemToLocation();
            InitFirstFrame(playerDataManager);
        
        
            worldHandler.ShipPlayer.transform.parent = transform;
            worldHandler.ShipPlayer.transform.parent = null;
        }


        public void LoadLocation()
        {
            CurrentSave = JsonConvert.DeserializeObject<Location>(File.ReadAllText(filesSystemHandler.CurrentLocationFile));
            var system = JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(filesSystemHandler.CacheSystemsFolder + CurrentSave.systemName + ".solar"));
            worldHandler.ChangeSolarSystem(system);
            SolarStaticBuilder.DrawAll(worldHandler.CurrentSolarSystem, transform, sunPrefab, planet, station, systemPoint, beltPoint, 15, false);
        }

        public void SetSystemToLocation()
        {
            foreach (var item in FindObjectsOfType<LineRenderer>())
            {
                item.enabled = false;
            }

            foreach (var item in FindObjectsOfType<WorldInteractivePoint>())
            {
                if (item.name != CurrentSave.locationName)
                {
                    Destroy(item.gameObject);
                }
            }
            foreach (var item in FindObjectsOfType<RotateAround>())
            {
                item.Rotate();
                if (item.GetComponent<TexturingScript>() != null)
                {
                    foreach (var coll in item.GetComponentsInChildren<SphereCollider>())
                    {
                        coll.enabled = false;
                    }
                }
                Destroy(item);
            }

            OnSetSystemToLocation.Invoke();
        }


        private void Update()
        {
            if (worldHandler.ShipPlayer)
            {
                transform.position = worldHandler.ShipPlayer.transform.position;
            }
        }

        public void InitFirstFrame(PlayerDataManager playerDataManager)
        {
            var location = MoveWorld();
            var locationObject = location.GetComponent<WorldInteractivePoint>();
            
            
            locationObject.InitLocation(playerDataManager, this);
            
            
            if (worldHandler.ShipPlayer.saves.ExKey("loc_start"))
            {
                worldHandler.ShipPlayer.transform.position = locationObject.SpawnPoint.position;
                worldHandler.ShipPlayer.transform.rotation = locationObject.SpawnPoint.rotation;
            }
            else if (worldHandler.ShipPlayer.saves.ExKey("loc_start_on_pit"))
            {
                var allPoints = CurrentLocationData.OrbitStation.Points.GetLandPoint();
                var point = allPoints[Random.Range(0, allPoints.Count)];
            
                worldHandler.ShipPlayer.transform.position = point.point.position;
                worldHandler.ShipPlayer.transform.rotation = point.point.rotation;
            
                worldHandler.ShipPlayer.land.SetLand(true, point.point.position, point.point.rotation);

                point.isFilled = true;
                point.isFilled = true;
            }

            SetSpaceObjectDistance();
        }
        

        public void SetSpaceObjectDistance()
        {
            if (worldHandler.ShipPlayer.saves.ExKey("loc_start"))
            {   
                worldHandler.ShipPlayer.saves.DelKey("loc_start");
            }else
            if (worldHandler.ShipPlayer.saves.ExKey("loc_start_on_pit"))
            {
                worldHandler.ShipPlayer.saves.DelKey("loc_start_on_pit");
            }
            else
            {
                foreach (Transform spaceObject in transform)
                {
                    spaceObject.transform.position += worldHandler.ShipPlayer.transform.position;
                } 
            }
        }
    
        public GameObject MoveWorld()
        {
            var location = GameObject.Find(CurrentSave.locationName);
            if (location != null)
            {
                foreach (Transform item in transform)
                {
                    item.position -= location.transform.position;
                }
                location.transform.parent = null;
            }

            return location;
        }

    
    
    
        public static void SaveLocationFile(string locName, LocationPoint.LocationType type, Dictionary<string, object> otherData = default)
        {
            var n = new Location
            {
                systemName = Path.GetFileNameWithoutExtension(SolarStaticBuilder.GetSystemFileName()),
                locationName = locName,
                type = type,
                otherKeys = otherData
            };
            File.WriteAllText(PlayerDataManager.Instance.FSHandler.CurrentLocationFile, JsonConvert.SerializeObject(n, Formatting.None, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore} ));
        }

        public static void RemoveLocationFile()
        {
            File.Delete(PlayerDataManager.Instance.FSHandler.CurrentLocationFile);
        }

        public void SetSpawnedLocation(GameObject location, SpawnedLocation.SpawnedLocationType type)
        {
            currentLocationData = new SpawnedLocation(location, type);
        }
    }
}