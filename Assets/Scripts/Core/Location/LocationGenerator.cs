using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Galaxy;
using Core.PlayerScripts;
using Core.Systems;
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
    public class LocationGenerator : MonoBehaviour
    {
        public GameObject player, planet, sunPrefab, station, systemPoint, beltPoint, botsLocation;
        public static Location CurrentSave;

        public Event OnSetSystemToLocation = new Event();

        private WorldDataHandler worldHandler;
        
        
        private void Awake()
        {
            worldHandler = PlayerDataManager.Instance.WorldHandler;
                
            World.SetScene(Scenes.Location);
            
        }
        private void OnEnable()
        {
            CurrentSave = null;
            OrbitalStationStaticBuilder.ClearEvent();
            if (FindObjectOfType<Player>() == null)
            {
                Instantiate(player.gameObject).GetComponent<Player>().Init();
            }


            if (!File.Exists(PlayerDataManager.Instance.FSHandler.CurrentLocationFile))
            {
                World.LoadLevel(Scenes.System);
                return;
            }


            LoadLocation();
            SetSystemToLocation();
            InitFirstFrame();
        
        
            Player.inst.transform.parent = transform;
            Player.inst.transform.parent = null;
        
            GetComponent<SolarSystemShips>().Init();
        }


        public void LoadLocation()
        {
            CurrentSave = JsonConvert.DeserializeObject<Location>(File.ReadAllText(PlayerDataManager.Instance.FSHandler.CurrentLocationFile));
            var system = JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(PlayerDataManager.Instance.FSHandler.CacheSystemsFolder + CurrentSave.systemName + ".solar"));
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
            if (Player.inst)
            {
                transform.position = Player.inst.transform.position;
            }
        }

        public void InitFirstFrame()
        {
            var location = MoveWorld();
            var locationObject = location.GetComponent<WorldInteractivePoint>();
            if (Player.inst.saves.ExKey("loc_start"))
            {
                locationObject.initEvent.AddListener(delegate
                {
                    Player.inst.transform.position = locationObject.spawnPoint.position;
                    Player.inst.transform.rotation = locationObject.spawnPoint.rotation;
                });
            }
            else if (Player.inst.saves.ExKey("loc_start_on_pit"))
            {
                locationObject.initEvent.AddListener(delegate
                {
                    var allPoints = WorldOrbitalStation.Instance.GetComponentInChildren<WorldOrbitalStationPoints>().GetLandPoint();
                    var point = allPoints[Random.Range(0, allPoints.Count)];
            
                    Player.inst.transform.position = point.point.position;
                    Player.inst.transform.rotation = point.point.rotation;
            
                    Player.inst.land.SetLand(true, point.point.position, point.point.rotation);

                    point.isFilled = true;
                });
            }
            locationObject.initEvent.Invoke();
        }

        private void Start()
        {
            SetSpaceObjectDistance();
        }

        public void SetSpaceObjectDistance()
        {
            if (Player.inst.saves.ExKey("loc_start"))
            {   
                Player.inst.saves.DelKey("loc_start");
            }else
            if (Player.inst.saves.ExKey("loc_start_on_pit"))
            {
                Player.inst.saves.DelKey("loc_start_on_pit");
            }
            else
            {
                foreach (Transform spaceObject in transform)
                {
                    spaceObject.transform.position += Player.inst.transform.position;
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
    }
}