using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


[System.Serializable]
public class Location
{
    public string systemName;
    public string locationName;
}

public class LocationGenerator : MonoBehaviour
{
    public GameObject player, planet, sunPrefab, station, systemPoint;
    public static Location CurrentSave;
    private bool initFirstFrame;

    private void Awake()
    {
        World.SetScene(Scenes.Location);
    }
    private void OnEnable()
    {
        CurrentSave = null;
        
        if (FindObjectOfType<Player>() == null)
        {
            Instantiate(player.gameObject).GetComponent<Player>().Init();
        }


        if (!File.Exists(PlayerDataManager.CurrentLocationFile))
        {
            World.LoadLevel(Scenes.System);
            return;
        }


        LoadLocation();
        SetSystemToLocation();
    }

    public void LoadLocation()
    {
        CurrentSave = JsonConvert.DeserializeObject<Location>(File.ReadAllText(PlayerDataManager.CurrentLocationFile));
        var system =
            JsonConvert.DeserializeObject<SolarSystem>(
                File.ReadAllText(PlayerDataManager.CacheSystemsFolder + CurrentSave.systemName + ".solar"));
        PlayerDataManager.CurrentSolarSystem = system;
        SolarSystemGenerator.DrawAll(PlayerDataManager.CurrentSolarSystem, transform, sunPrefab, planet, station,
            systemPoint, 40,
            false);
    }

    public void SetSystemToLocation()
    {
        foreach (var item in FindObjectsOfType<LineRenderer>())
        {
            item.enabled = false;
        }

        foreach (var item in FindObjectsOfType<WorldOrbitalStation>())
        {
            if (item.name != CurrentSave.locationName)
            {
                Destroy(item.gameObject);
            }
        }
        foreach (var item in FindObjectsOfType<RotateAround>())
        {
            Destroy(item);
        }
    }


    private void Update()
    {
        InitFirstFrame();

        transform.position = Player.inst.transform.position;
    }

    public void InitFirstFrame()
    {
        if (!initFirstFrame)
        {
            var location = MoveWorld();
            
            
            if (Player.inst.saves.ExKey("loc_start"))
            {
                var station = location.GetComponent<WorldOrbitalStation>();
                Player.inst.transform.position = station.spawnPoint.position;
                Player.inst.transform.rotation = station.spawnPoint.rotation;
                Player.inst.saves.DelKey("loc_start");
            }
            
            initFirstFrame = true;
        }
    }

    public GameObject MoveWorld()
    {
        var location = GameObject.Find(CurrentSave.locationName);
        foreach (Transform item in transform)
        {
            item.position -= location.transform.position;
        }

        location.transform.parent = null;

        return location;
    }

    
    
    
    public static void SaveLocationFile(string locName)
    {
        var n = new Location()
        {
            systemName = Path.GetFileNameWithoutExtension(SolarSystemGenerator.GetSystemFileName()),
            locationName = locName,
        };
        File.WriteAllText(PlayerDataManager.CurrentLocationFile, JsonConvert.SerializeObject(n));
    }

    public static void RemoveLocationFile()
    {
        File.Delete(PlayerDataManager.CurrentLocationFile);
    }
}
