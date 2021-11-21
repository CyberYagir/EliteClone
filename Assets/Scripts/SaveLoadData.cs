using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class PlayerData
{
    public Vector3 worldPos;
    public Vector3 pos;
    public Vector3 rot;
    public float fuel, health, shields, speed;

    public Dictionary<string, object> keys;
}



public class SaveLoadData : MonoBehaviour
{
    Dictionary<string, object> keys = new Dictionary<string, object>();
    private void Start()
    {
        Load();
    }

    public bool ExKey(string name)
    {
        return keys.ContainsKey(name);
    }
    public void SetKey(string name, object value)
    {
        if (keys.ContainsKey(name))
        {
            ChangeKey(name, value);
        }
        else
        {
            keys.Add(name, value);
            Save();
        }
    }

    public void ChangeKey(string name, object value)
    {
        keys[name] = value;
        Save();
    }

    public void DelKey(string name)
    {
        keys.Remove(name);
        Save();
    }
    
    public void Load()
    {
        if (File.Exists(PlayerDataManager.playerDataFile))
        {
            var json = File.ReadAllText(PlayerDataManager.playerDataFile);
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(json);

            var world = GameObject.FindGameObjectWithTag("WorldHolder");
            var p = Player.inst;

            world.transform.position = playerData.worldPos;

            p.transform.position = playerData.pos;
            p.transform.eulerAngles = playerData.rot;
            p.Ship().fuel.value = playerData.fuel;
            p.Ship().hp.value = playerData.health;
            p.Ship().shields.value = playerData.shields;
            p.control.speed = playerData.speed;
            keys = playerData.keys;
        }
        else
        {
            Save();
        }
    }

    public void Save()
    {
        var world = GameObject.FindGameObjectWithTag("WorldHolder");
        var p = Player.inst;
        var playerData = new PlayerData()
        {
            fuel = p.Ship().fuel.value,
            health = p.Ship().hp.value,
            shields = p.Ship().shields.value,
            speed = p.control.speed,
            pos = p.transform.position,
            rot = p.transform.eulerAngles,
            worldPos = world.transform.position,
            keys = keys
        };

        var data = JsonConvert.SerializeObject(playerData, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        File.WriteAllText(PlayerDataManager.playerDataFile, data);
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
