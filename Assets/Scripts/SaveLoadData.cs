using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class PlayerData
{
    public Vector3 WorldPos;
    public Vector3 Pos;
    public Vector3 Rot;
    public float Fuel, Health, Shields, Speed;

    public Dictionary<string, object> Keys;

    public LandLocation IsLanded;
}


[System.Serializable]
public class LandLocation
{
    public Vector3 pos;
    public Quaternion rot;
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
        if (File.Exists(PlayerDataManager.PlayerDataFile))
        {
            var json = File.ReadAllText(PlayerDataManager.PlayerDataFile);
            PlayerData playerData = JsonConvert.DeserializeObject<PlayerData>(json);

            var world = GameObject.FindGameObjectWithTag("WorldHolder");
            var p = Player.inst;

            if (playerData != null)
            {
                world.transform.position = playerData.WorldPos;

                p.transform.position = playerData.Pos;
                p.transform.eulerAngles = playerData.Rot;
                
                p.Ship().fuel.value = playerData.Fuel;
                p.Ship().hp.value = playerData.Health;
                p.Ship().shields.value = playerData.Shields;
                p.control.speed = playerData.Speed;
                keys = playerData.Keys;
                p.land.SetLand(playerData.IsLanded);
            }
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
            Fuel = p.Ship().fuel.value,
            Health = p.Ship().hp.value,
            Shields = p.Ship().shields.value,
            Speed = p.control.speed,
            Pos = p.transform.position,
            Rot = p.transform.eulerAngles,
            WorldPos = world.transform.position,
            Keys = keys,
            IsLanded = p.land.GetLand()
        };

        var data = JsonConvert.SerializeObject(playerData, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        File.WriteAllText(PlayerDataManager.PlayerDataFile, data);
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
