using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Random = System.Random;

public class SolarSystemShips : MonoBehaviour
{
    public static SolarSystemShips Instance;

    public Dictionary<string, List<HumanShipDead>> deadList { get; private set; } = new Dictionary<string, List<HumanShipDead>>();

    [System.Serializable]
    public class LocationHolder
    {
        public string locationName;
        public List<HumanShip> humans = new List<HumanShip>();

        public LocationHolder(string name)
        {
            locationName = name;
        }
    }

    [System.Serializable]
    public class HumanShip
    {
        public int uniqID;
        public int shipID;
        public string firstName, lastName;

        public HumanShip(Random rnd, int maxID, int uid)
        {
            NamesHolder.Init();
            shipID = rnd.Next(0, maxID);
            firstName = NamesHolder.GetFirstName(rnd);
            lastName = NamesHolder.GetLastName(rnd);
            uniqID = uid;
        }
    }

    public class HumanShipDead
    {
        public int uniqID;
        public string locationName;
        public DVector deadPos = new DVector();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void LoadDeads()
    {
        if (File.Exists(PlayerDataManager.DeadsNPCFile))
        {
            deadList = JsonConvert.DeserializeObject<Dictionary<string, List<HumanShipDead>>>(File.ReadAllText(PlayerDataManager.DeadsNPCFile));
        }
    }
    public void SaveDeads()
    {
        File.WriteAllText(PlayerDataManager.DeadsNPCFile, JsonConvert.SerializeObject(deadList));
    }

    public void AddDead(BotBuilder builder)
    {
        if (!deadList.ContainsKey(PlayerDataManager.CurrentSolarSystem.name))
        {
            deadList.Add(PlayerDataManager.CurrentSolarSystem.name, new List<HumanShipDead>());
        }

        deadList[PlayerDataManager.CurrentSolarSystem.name].Add(new HumanShipDead() {locationName = LocationGenerator.CurrentSave.locationName, uniqID = builder.uniqID, deadPos = DVector.FromVector3(builder.transform.position)});
        SaveDeads();
    }

    public int GetSpeed()
    {
        var seed = (int) (PlayerDataManager.CurrentSolarSystem.position.x + PlayerDataManager.CurrentSolarSystem.position.y + PlayerDataManager.CurrentSolarSystem.position.z);
        return seed;
    }

    public int GetShipsCount()
    {
        return new Random(GetSpeed()).Next(10, 30);
    }


    void InitShipsPoses()
    {
        for (int i = 0; i < PlayerDataManager.CurrentSolarSystem.stations.Count; i++)
        {
            locations.Add(new LocationHolder(PlayerDataManager.CurrentSolarSystem.stations[i].name));
        }

        for (int i = 0; i < PlayerDataManager.CurrentSolarSystem.belts.Count; i++)
        {
            locations.Add(new LocationHolder(PlayerDataManager.CurrentSolarSystem.belts[i].name));
        }

        var rnd = new Random(GetSpeed());
        var count = GetShipsCount();
        if (locations.Count < 2)
        {
            count = Mathf.Clamp(count, 2, 10);
        }
        var positionsRnd = new Random(GetSpeed() + SaveLoadData.GetCurrentSaveSeed());
        for (int i = 0; i < count; i++)
        {
            var locID = positionsRnd.Next(-1, locations.Count);
            var ship = new HumanShip(rnd, botPrefab.ships.Count, GetSpeed() + i);
            if (locID >= 0)
            {
                if (locations[locID].humans.Count < 3)
                {
                    locations[locID].humans.Add(ship);
                    continue;
                }
            }
            
            ships.Add(ship);
        }

    }

    [SerializeField] private BotVisual botPrefab;
    [SerializeField] private List<LocationHolder> locations = new List<LocationHolder>();
    [SerializeField] private List<HumanShip> ships = new List<HumanShip>();

    public void Init()
    {
        if (PlayerDataManager.CurrentSolarSystem != null)
        {
            LoadDeads();
            InitShipsPoses();
            CreateBots();
        }
    }
    public void CreateBots()
    {
        var isLocation = World.Scene == Scenes.Location;

        if (isLocation)
        {
            var location = locations.Find(x => x.locationName == LocationGenerator.CurrentSave.locationName);
            if (location != null)
            {
                for (int i = 0; i < location.humans.Count; i++)
                {
                    if (!deadList.ContainsKey(PlayerDataManager.CurrentSolarSystem.name) || deadList[PlayerDataManager.CurrentSolarSystem.name].Find(x => x.uniqID == location.humans[i].uniqID) == null)
                    {
                        var pos = UnityEngine.Random.insideUnitSphere * 1000;
                        var bot = Instantiate(botPrefab.gameObject, pos, Quaternion.Euler(-pos)).GetComponent<BotBuilder>();
                        bot.InitBot(false, NamesHolder.ToUpperFist(location.humans[i].firstName), NamesHolder.ToUpperFist(location.humans[i].lastName));
                        bot.uniqID = location.humans[i].uniqID;
                        bot.GetVisual().SetVisual(location.humans[i].shipID);
                        bot.AddContact(false);
                        bot.GetShield().isActive = true;
                        bot.SetBehaviour(BotBuilder.BotState.Moving);
                    }
                }
            }
        }
    }
}
