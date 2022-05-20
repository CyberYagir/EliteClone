using System;
using System.Collections.Generic;
using System.IO;
using Core.Bot;
using Core.Galaxy;
using Core.Game;
using Core.PlayerScripts;
using Core.Systems;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Random = System.Random;

namespace Core.Location
{
    public class SolarSystemShips : Singleton<SolarSystemShips>
    {
        public static Dictionary<string, List<HumanShipDead>> deadList { get; private set; } = new Dictionary<string, List<HumanShipDead>>();
    
    
        [SerializeField] private GameObject botPrefab, botLocation, garbageContact;
        [SerializeField] private BotVisual botPrefabVisuals;
        [SerializeField] private List<LocationHolder> locations = new List<LocationHolder>();
        [SerializeField] private List<HumanShip> ships = new List<HumanShip>();
        [SerializeField] private List<HumanShip> allships = new List<HumanShip>();
        private bool spawnEnviroment;
    

        [Serializable]
        public class LocationHolder
        {
            public string locationName;
            public List<HumanShip> humans = new List<HumanShip>();

            public LocationHolder(string name)
            {
                locationName = name;
            }
        }

        [Serializable]
        public class HumanShip
        {
            public int uniqID;
            public int shipID;
            public string firstName, lastName;
            public int fraction;

            public HumanShip(Random rnd, int maxID, int uid)
            {
                NamesHolder.Init();
                shipID = rnd.Next(0, maxID);
                firstName = NamesHolder.GetFirstName(rnd);
                lastName = NamesHolder.GetLastName(rnd);
                uniqID = uid;
                fraction = rnd.Next(0, WorldDataItem.Fractions.Count);
            }
        }

        public class HumanShipDead
        {
            public int uniqID;
            public string locationName, botFullName;
            public DVector deadPos;
        }

        private void Awake()
        {
            Single(this);
        }

        public static void LoadDeads()
        {
            if (File.Exists(PlayerDataManager.DeadsNPCFile) && deadList.Count == 0)
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

            deadList[PlayerDataManager.CurrentSolarSystem.name].Add(new HumanShipDead {botFullName = builder.transform.name, locationName = LocationGenerator.CurrentSave.locationName, uniqID = builder.uniqID, deadPos = DVector.FromVector3(builder.transform.position)});

            ExplodeShip(builder);
        
            SaveDeads();
        }

        public void ExplodeShip(BotBuilder builder)
        {
            var garbage = CreateGarbage(builder.transform.name, builder.GetShip().shipName, builder.transform.position, true);
            foreach (Rigidbody item in garbage.GetComponentsInChildren<Rigidbody>())
            {
                item.AddExplosionForce(20, builder.transform.position, 10);   
            }
        }
        
        public GameObject CreateGarbage(string botName,string shipName, Vector3 pos, bool triggerContactEvent = false)
        {
            var ship = ItemsManager.GetShipItem(shipName);
            var wreckage = Instantiate(ship.shipWreckage.gameObject, pos, Quaternion.Euler(UnityEngine.Random.insideUnitSphere * 360));
            wreckage.transform.localScale = Vector3.one * 0.2f;
        
            foreach (Transform item in wreckage.transform)
            {
                var filter = item.GetComponent<MeshFilter>();
                if (filter)
                {
                    var mesh = filter.mesh;
                    if (mesh.triangles.Length > 15)
                    {
                        if (item.GetComponent<MeshRenderer>())
                        {
                            var rb = item.gameObject.AddComponent<Rigidbody>();
                            rb.useGravity = false;
                            item.gameObject.layer = LayerMask.NameToLayer("Main");
                            item.gameObject.AddComponent<MeshCollider>().convex = true;
                        }
                    }
                }
            }
            wreckage.name = botName + " Shipwreck";

            var contact = Instantiate(garbageContact, wreckage.transform.position, wreckage.transform.rotation).GetComponent<ContactObject>();
            contact.transform.parent = wreckage.transform;
            contact.transform.name = wreckage.name;
            contact.Init(triggerContactEvent);
        
            var light = new GameObject("Light");
            light.transform.parent = wreckage.transform;
            light.transform.localPosition = Vector3.zero;
            var sLight = light.AddComponent<Light>();
            var aLight = light.AddComponent<HDAdditionalLightData>();
            aLight.intensity = 800;
            sLight.type = LightType.Point;
            sLight.color = Color.red;

        
            return wreckage;
        }

       
        public static int GetSeed(Vector3 pos)
        {
            var seed = (int) (pos.x + pos.y + pos.z);
            return seed;
        }

        public static int GetShipsCount(Vector3 pos, int stationsCount)
        {
            return new Random(GetSeed(pos)).Next(stationsCount, stationsCount * 6);
        }


        public void InitShipsPoses()
        {
            allships = new List<HumanShip>();
            ships = new List<HumanShip>();
            locations = new List<LocationHolder>();
            for (int i = 0; i < PlayerDataManager.CurrentSolarSystem.stations.Count; i++)
            {
                locations.Add(new LocationHolder(PlayerDataManager.CurrentSolarSystem.stations[i].name));
            }

            for (int i = 0; i < PlayerDataManager.CurrentSolarSystem.belts.Count; i++)
            {
                locations.Add(new LocationHolder(PlayerDataManager.CurrentSolarSystem.belts[i].name));
            }
            var count = GetShipsCount(PlayerDataManager.CurrentSolarSystem.position.ToVector(), PlayerDataManager.CurrentSolarSystem.stations.Count);
            if (locations.Count < 2)
            {
                count = Mathf.Clamp(count, 2, 10);
            }

            var seed = GetSeed(PlayerDataManager.CurrentSolarSystem.position.ToVector());
            var positionsRnd = new Random(seed + SaveLoadData.GetCurrentSaveSeed());
            var shipsGened = GetShips(PlayerDataManager.CurrentSolarSystem);
            
            
            for (int i = 0; i < shipsGened.Count; i++)
            {
                var locID = positionsRnd.Next(-1, locations.Count);
                if (locID >= 0)
                {
                    if (locations[locID].humans.Count < 3)
                    {
                        locations[locID].humans.Add(shipsGened[i]);
                        allships.Add(shipsGened[i]);
                        continue;
                    }
                }
                allships.Add(shipsGened[i]);
                ships.Add(shipsGened[i]);
            }

        }

        public static List<HumanShip> GetShips(SolarSystem solar)
        {
            var ships = new List<HumanShip>();
            var count = GetShipsCount(solar.position.ToVector(), solar.stations.Count);
            if (solar.stations.Count + solar.belts.Count < 2)
            {
                count = Mathf.Clamp(count, 2, 10);
            }
            
            var seed = GetSeed(solar.position.ToVector());
            var rnd = new Random(seed);
            
            
            for (int i = 0; i < count; i++)
            {
                var ship = new HumanShip(rnd, rnd.Next(0,4), seed + i);
                ships.Add(ship);
            }

            return ships;
        }


        public void InitPre()
        {
            if (PlayerDataManager.CurrentSolarSystem != null)
            {
                Single(this);
                LoadDeads();
                InitShipsPoses();
            }
        }

        public ref List<HumanShip> GetShips()
        {
            return ref ships;
        }
    
        public void Init()
        {
            if (PlayerDataManager.CurrentSolarSystem != null)
            {
                InitPre();
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
                            if (!IsDead(location.humans[i].uniqID))
                            {
                                CreateBot(location.humans[i]);
                            }
                        }
                    }
                }
                SpawnSystemPoints();
            }
            else
            {
                SpawnEnviromentBots(botPrefab);
            }
        }

        public bool IsDead(int id)
        {
            bool dead = false;
            foreach (var locations in deadList)
            {
                if (locations.Value.Find(x => x.uniqID == id) != null)
                {
                    dead = true;
                    break;
                }
            }

            return dead;
        }

        public BotBuilder CreateBot(HumanShip ship, BotBuilder.BotState state = BotBuilder.BotState.Moving)
        {
            var pos = UnityEngine.Random.insideUnitSphere * 1000;
            var bot = Instantiate(botPrefab.gameObject, pos, Quaternion.Euler(-pos)).GetComponent<BotBuilder>();
            if (ship != null)
            {
                bot.InitBot(NamesHolder.ToUpperFist(ship.firstName), NamesHolder.ToUpperFist(ship.lastName));
                bot.GetVisual().SetVisual(ship.shipID);
                bot.uniqID = ship.uniqID;
            }

            bot.AddContact(false);
            bot.GetShield().isActive = true;
            bot.SetBehaviour(state);

            return bot;
        }

        public void OnLocationInit()
        {
            InitPre();
            SpawnSystemPoints();
        
            foreach (var item in points)
            {
                if (item.name != LocationGenerator.CurrentSave.locationName)
                {
                    Destroy(item.gameObject);
                }
                else
                {
                    item.transform.parent = transform;
                }
            }
        }

        public void SpawnSystemPoints()
        {
            if (!spawnEnviroment)
            {
                SpawnEnviromentBots(botLocation);
                spawnEnviroment = true;
            }
        }

        public List<WorldInteractivePoint> points = new List<WorldInteractivePoint>();
        public void SpawnEnviromentBots(GameObject prefab)
        {
            for (int i = 0; i < ships.Count; i++)
            {
                var botName = NamesHolder.ToUpperFist(ships[i].firstName) + " " + NamesHolder.ToUpperFist(ships[i].lastName);
                if (!IsDead(ships[i].uniqID) || (World.Scene == Scenes.Location && LocationGenerator.CurrentSave.locationName == botName))
                {
                    var rnd = new Random(ships[i].uniqID + Mathf.RoundToInt((float) Player.inst.saves.GetTime() / 60f / 60f));
                    var pos = (new Vector3(rnd.Next(-100, 100), rnd.Next(-100, 100), rnd.Next(-100, 100)) / 100f) * 10000f;
                    var worldBot = Instantiate(prefab.gameObject, pos, Quaternion.identity);
                    worldBot.name = botName;
                    if (worldBot.TryGetComponent(out ContactObject contact))
                    {
                        contact.Init(false);
                        var loc = worldBot.GetComponentInChildren<LocationPoint>();

                        var data = new Dictionary<string, object>();
                        data.Add("tag", "ships");

                        LocationBotType type;
                        do
                        {
                            type = (LocationBotType) rnd.Next(0, Enum.GetNames(typeof(LocationBotType)).Length);
                            if (type != LocationBotType.OCG)
                            {
                                break;
                            }
                        } while (type == LocationBotType.OCG && (ships[i].fraction != WorldDataItem.Fractions.NameToID("Pirates") || ships[i].fraction != WorldDataItem.Fractions.NameToID("OCG")));


                        data.Add("tag-type", type);
                        data.Add("uniqID", ships[i].uniqID);


                        loc.SetData(data);
                    }
                    else
                    {
                        points.Add(worldBot.GetComponent<WorldInteractivePoint>());
                    }
                }
            }

            SpawnGarbage();
        

            spawnEnviroment = true;
        }


        public void SpawnGarbage()
        {
            if (World.Scene == Scenes.Location)
            {
                if (deadList.ContainsKey(PlayerDataManager.CurrentSolarSystem.name))
                {
                    var deadOnLoc = deadList[PlayerDataManager.CurrentSolarSystem.name].FindAll(x => x.locationName == LocationGenerator.CurrentSave.locationName);
                    for (int i = 0; i < deadOnLoc.Count; i++)
                    {
                        var bot = allships.Find(x => x.uniqID == deadOnLoc[i].uniqID);
                        if (bot != null)
                        {
                            CreateGarbage(deadOnLoc[i].botFullName, ItemsManager.GetShipItem(bot.shipID).shipName, deadOnLoc[i].deadPos.ToVector());
                        }
                    }
                }
            }
        }
    }
}
