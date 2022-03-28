using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Game;
using Core.Player;
using Newtonsoft.Json;
using UnityEngine;

namespace Core
{
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
        public decimal playedTime = 0;
        public int startSaveTime = 0;
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
        private static decimal startTime, playedTime;
        private static int startSaveTime;
        private void Awake()
        {
            if (Player.Player.inst)
            {
                Load();
            }
        }

        public static int GetCurrentSaveSeed()
        {
            return Mathf.FloorToInt((((float)startTime + (float)playedTime) * 2) / 60f / 60f) + startSaveTime;
        }
    
        private void Update()
        {
            playedTime += (decimal)Time.deltaTime;
        }

        #region Keys
   
        public bool ExKey(string name)
        {
            return keys.ContainsKey(name);
        }
        public void SetKey(string name, object value, bool save = true)
        {
            if (keys.ContainsKey(name))
            {
                ChangeKey(name, value, save);
            }
            else
            {
                keys.Add(name, value);
                if (save)
                {
                    Save();
                }
            }
        }

        public void ChangeKey(string name, object value, bool save = true)
        {
            keys[name] = value;
            if (save)
                Save();
        }

        public void DelKey(string name, bool save = true)
        {
            keys.Remove(name);
            if (save)
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

        public decimal GetTime()
        {
            return startTime + playedTime;
        }

        public Dictionary<string, object> GetKeys()
        {
            return keys;
        }
        public void Load()
        {
            var playerData = LoadData();
            if (playerData != null)
            {
                var world = GameObject.FindGameObjectWithTag("WorldHolder");
                var p = Player.Player.inst;

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
                
                    startTime = playerData.playedTime;
                
                    shipsInStations = playerData.shipsInStations;

                    startTime = playerData.playedTime;

                    startSaveTime = playerData.startSaveTime;
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
                keys = playerData.Keys;
                shipsInStations = playerData.shipsInStations;
                return playerData;
            }
            return null;
        }
        public void Save()
        {
            var world = GameObject.FindGameObjectWithTag("WorldHolder");
            var p = Player.Player.inst;
            if (startSaveTime == 0)
            {
                startSaveTime = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour * DateTime.Now.Minute * DateTime.Now.Millisecond;
            }
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
                shipsInStations = shipsInStations,
                playedTime = GetTime(),
                startSaveTime = startSaveTime
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
            if (Player.Player.inst)
            {
                Save();
            }
        }
    
        #endregion
    }
}