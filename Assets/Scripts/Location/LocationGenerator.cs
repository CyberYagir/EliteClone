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
    public Location current;
    bool init;

    private void Awake()
    {
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
        var system = JsonConvert.DeserializeObject<SolarSystem>(
            File.ReadAllText(PlayerDataManager.cacheSystemsFolder + current.systemName + ".solar"));
        
        PlayerDataManager.currentSolarSystem = system;
        
        
        SolarSystemGenerator.DrawAll(PlayerDataManager.currentSolarSystem, transform, sunPrefab, planet, station, systemPoint, 40,
            false);
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
    }

    private void Update()
    {
        if (!init)
        {
            var location = GameObject.Find(current.locationName);
            foreach (Transform item in transform)
            {
                item.position -= location.transform.position;
            }

            location.transform.parent = null;
            if (Player.inst.saves.ExKey("loc_start"))
            {
                Player.inst.transform.position = location.GetComponent<WorldOrbitalStation>().spawnPoint.position;
                Player.inst.transform.rotation = location.GetComponent<WorldOrbitalStation>().spawnPoint.rotation;
                Player.inst.saves.DelKey("loc_start");
            }

            foreach (var item in FindObjectsOfType<RotateAround>())
            {
                item.enabled = false;
            }

            init = true;
        }

        transform.position = Player.inst.transform.position;
    }

    public static void SaveLocationFile(Location loc)
    {
        File.WriteAllText(PlayerDataManager.currentLocationFile, JsonConvert.SerializeObject(loc));
    }

    public static void RemoveLocationFile()
    {
        File.Delete(PlayerDataManager.currentLocationFile);
    }
}
