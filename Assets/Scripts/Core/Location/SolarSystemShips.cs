using System;
using System.Collections.Generic;
using System.IO;
using Core.Bot;
using Core.Galaxy;
using Core.Game;
using Core.PlayerScripts;
using Core.Systems;
using Core.Systems.InteractivePoints;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Random = System.Random;

namespace Core.Location
{
    public class SolarSystemShips : StartupObject 
    {
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
            public BotBuilder builder;
            public HumanShip(Random rnd, int maxID, int uid)
            {
                NamesHolder.Init();
                shipID = rnd.Next(0, maxID);
                firstName = NamesHolder.GetFirstName(rnd);
                lastName = NamesHolder.GetLastName(rnd);
                uniqID = uid;
                fraction = rnd.Next(0, WorldDataItem.Fractions.Count);
                builder = builder;
            }
        }

        public class HumanShipDead
        {
            public int uniqID;
            public string locationName, botFullName;
            public DVector deadPos;
        }

        
        public class ShipsData
        {
            public List<HumanShip> allships = new List<HumanShip>();
            public List<HumanShip> ships = new List<HumanShip>();
            public List<LocationHolder> locations = new List<LocationHolder>();

        }
        
    
        [SerializeField] private GameObject botPrefab, botLocation, garbageContact;
        [SerializeField] private BotVisual botPrefabVisuals;
        [SerializeField] private List<LocationHolder> locations = new List<LocationHolder>();
        [SerializeField] private List<HumanShip> ships = new List<HumanShip>();
        [SerializeField] private List<HumanShip> allships = new List<HumanShip>();
        private bool spawnEnviroment;


        private FilesSystemHandler filesSystemHandler;
        private WorldDataHandler worldHandler;


        public override void Init(PlayerDataManager playerDataManager)
        {
            base.Init(playerDataManager);
            worldHandler = playerDataManager.WorldHandler;
            filesSystemHandler = playerDataManager.FSHandler;
            worldHandler.SetSystemShips(this);
            if (worldHandler.CurrentSolarSystem != null)
            {
                InitPre();
                CreateBots();
            }
        }

        public void SaveDeads()
        {
            File.WriteAllText(filesSystemHandler.DeadsNpcFile, JsonConvert.SerializeObject(SolarSystemShipsStaticBuilder.deadList));
        }

        public void AddDead(BotBuilder builder)
        {
            var system = worldHandler.CurrentSolarSystem;
            if (!SolarSystemShipsStaticBuilder.deadList.ContainsKey(system.name))
            {
                SolarSystemShipsStaticBuilder.deadList.Add(system.name, new List<HumanShipDead>());
            }

            SolarSystemShipsStaticBuilder.deadList[system.name].Add(new HumanShipDead {botFullName = builder.transform.name, locationName = LocationGenerator.CurrentSave.locationName, uniqID = builder.uniqID, deadPos = DVector.FromVector3(builder.transform.position)});

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
        
        
        public void InitPre()
        {
            worldHandler = PlayerDataManager.Instance.WorldHandler;
            filesSystemHandler = PlayerDataManager.Instance.FSHandler;
            
            if (worldHandler.CurrentSolarSystem != null)
            {
                SolarSystemShipsStaticBuilder.LoadDeads();
                var data = SolarSystemShipsStaticBuilder.InitShipsPoses(worldHandler.CurrentSolarSystem.name);

                ships = data.ships;
                allships = data.allships;
                locations = data.locations;
            }
        }

        public ref List<HumanShip> GetShips()
        {
            return ref ships;
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
                        if (!SolarSystemShipsStaticBuilder.deadList.ContainsKey(worldHandler.CurrentSolarSystem.name) || SolarSystemShipsStaticBuilder.deadList[worldHandler.CurrentSolarSystem.name].Find(x => x.uniqID == location.humans[i].uniqID) == null)
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
            foreach (var locations in SolarSystemShipsStaticBuilder.deadList)
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
                bot.InitBot(worldHandler, NamesHolder.ToUpperFist(ship.firstName), NamesHolder.ToUpperFist(ship.lastName), this);
                bot.GetVisual().SetVisual(ship.shipID);
                bot.uniqID = ship.uniqID;
                bot.SetHuman(ship);
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
                    var rnd = new Random(ships[i].uniqID + Mathf.RoundToInt((float) worldHandler.ShipPlayer.SaveData.GetTime() / 60f / 60f));
                    var pos = (new Vector3(rnd.Next(-100, 100), rnd.Next(-100, 100), rnd.Next(-100, 100)) / 100f) * 10000f;
                    var worldBot = Instantiate(prefab.gameObject, pos, Quaternion.identity);
                    worldBot.name = botName;
                    if (worldBot.TryGetComponent(out ContactObject contact))
                    {
                        contact.Init(false);
                        var loc = worldBot.GetComponentInChildren<LocationPoint>();

                        if (loc)
                        {
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
                if (SolarSystemShipsStaticBuilder.deadList.ContainsKey(worldHandler.CurrentSolarSystem.name))
                {
                    var deadOnLoc = SolarSystemShipsStaticBuilder.deadList[worldHandler.CurrentSolarSystem.name].FindAll(x => x.locationName == LocationGenerator.CurrentSave.locationName);
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
