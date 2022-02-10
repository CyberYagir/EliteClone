using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Game;
using Quests;
using UnityEngine;
public class PlayerData
{
    public Vector3 WorldPos;
    public Vector3 Pos;
    public Vector3 Rot;
    public float Speed;
    public ShipData Ship;
    public Dictionary<string, object> Keys;
    public LandLocation IsLanded;
    public List<AppliedQuests.QuestData> quests = new List<AppliedQuests.QuestData>();
    public List<Cargo.ItemData> items = new List<Cargo.ItemData>();
    public Dictionary<string, List<ShipData>> shipsInStations = new Dictionary<string, List<ShipData>>();
}


[System.Serializable]
public class LandLocation
{
    public Vector3 pos;
    public Quaternion rot;
}

public class SaveLoadData : MonoBehaviour
{
    private Dictionary<string, object> keys = new Dictionary<string, object>();
    private Dictionary<string, List<ShipData>> shipsInStations = new Dictionary<string, List<ShipData>>();
    private void Awake()
    {
        if (Player.inst)
        {
            Load();
        }
    }

    #region Keys
   
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

    #endregion

    #region Ships

    public Dictionary<string, List<ShipData>> GetStorageShip() => shipsInStations.ToDictionary(entry => entry.Key, entry => entry.Value);
    public void SetStorageShips(Dictionary<string, List<ShipData>> ships) => shipsInStations = ships;

    public void AddStorageShip(string stationName, ItemShip ship)
    {
        if (shipsInStations.ContainsKey(stationName))
        {
            shipsInStations[stationName].Add(ship.SaveShip());
        }
        else
        {
            shipsInStations.Add(stationName, new List<ShipData>());
            AddStorageShip(stationName, ship);
        }
    }

    public bool RemoveStorageShip(string stationName, ItemShip ship)
    {
        if (shipsInStations.ContainsKey(stationName))
        {
            if (shipsInStations[stationName].RemoveAll(x=>x.shipID == ship.shipID) != 0)
                return true;
        }

        return false;
    }
    
    
    #endregion

    #region SaveLoad
    public void Load()
    {
        var playerData = LoadData();
        if (playerData != null)
        {
            var world = GameObject.FindGameObjectWithTag("WorldHolder");
            var p = Player.inst;

            if (playerData != null)
            {
                world.transform.position = playerData.WorldPos;

                p.transform.position = playerData.Pos;
                p.transform.eulerAngles = playerData.Rot;

                p.LoadShip(playerData.Ship);

                p.control.speed = playerData.Speed;
                keys = playerData.Keys;
                p.land.SetLand(playerData.IsLanded);
                p.quests.LoadList(playerData.quests);
                p.cargo.LoadData(playerData.items);
                
                shipsInStations = playerData.shipsInStations;
            }
        }
        else
        {
            Save();
        }
    }
    public PlayerData LoadData()
    {
        if (File.Exists(PlayerDataManager.PlayerDataFile))
        {
            var json = File.ReadAllText(PlayerDataManager.PlayerDataFile);
            var playerData = JsonConvert.DeserializeObject<PlayerData>(json);
            shipsInStations = playerData.shipsInStations;
            return playerData;
        }
        return null;
    }
    public void Save()
    {
        var world = GameObject.FindGameObjectWithTag("WorldHolder");
        var p = Player.inst;
        var playerData = new PlayerData()
        {
            Ship = p.Ship().SaveShip(),
            Speed = p.control.speed,
            Pos = p.transform.position,
            Rot = p.transform.eulerAngles,
            WorldPos = world.transform.position,
            Keys = keys,
            IsLanded = p.land.GetLand(),
            quests = p.quests.GetData(),
            items = p.cargo.GetData(),
            shipsInStations = shipsInStations
        };
        SaveData(playerData);
    }

    public void SaveData(PlayerData playerData)
    {
        var data = JsonConvert.SerializeObject(playerData, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        File.WriteAllText(PlayerDataManager.PlayerDataFile, data);
    }
    
    
    private void OnApplicationQuit()
    {
        if (Player.inst)
        {
            Save();
        }
    }
    
    #endregion
}
