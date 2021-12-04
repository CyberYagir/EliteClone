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
    public static Location current;
    private bool init;

    private void Awake()
    {
        current = null;
        
        if (FindObjectOfType<Player>() == null)
        {
            Instantiate(player.gameObject).GetComponent<Player>().Init();
        }


        if (!File.Exists(PlayerDataManager.currentLocationFile))
        {
            Application.LoadLevel("System");
            return;
        }

        current = JsonConvert.DeserializeObject<Location>(File.ReadAllText(PlayerDataManager.currentLocationFile));
        
        
        var system = JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(PlayerDataManager.cacheSystemsFolder + current.systemName + ".solar"));
        
        
        PlayerDataManager.currentSolarSystem = system;
        SolarSystemGenerator.DrawAll(PlayerDataManager.currentSolarSystem, transform, sunPrefab, planet, station, systemPoint, 40,
            false);

        SetSystemToLocation();
    }

    public void SetSystemToLocation()
    {
        foreach (var item in FindObjectsOfType<LineRenderer>())
        {
            item.enabled = false;
        }

        foreach (var item in FindObjectsOfType<WorldOrbitalStation>())
        {
            if (item.name != current.locationName)
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
        if (!init)
        {
            var location = MoveWorld();
            
            
            if (Player.inst.saves.ExKey("loc_start"))
            {
                var station = location.GetComponent<WorldOrbitalStation>();
                Player.inst.transform.position = station.spawnPoint.position;
                Player.inst.transform.rotation = station.spawnPoint.rotation;
                Player.inst.saves.DelKey("loc_start");
            }
            
            init = true;
        }
    }

    public GameObject MoveWorld()
    {
        var location = GameObject.Find(current.locationName);
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
        File.WriteAllText(PlayerDataManager.currentLocationFile, JsonConvert.SerializeObject(n));
    }

    public static void RemoveLocationFile()
    {
        File.Delete(PlayerDataManager.currentLocationFile);
    }
}
