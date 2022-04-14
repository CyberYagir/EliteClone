using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Systems;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Galaxy
{
    public class GalaxyGenerator : MonoBehaviour
    {
        public static Dictionary<string, SolarSystem> systems;
    
        public static float maxY = 5000, minY = -5000;
        public static float maxRadius = 100000, minRadius = 30000;
        public static int maxSystemsCount = 6000, minSystemsCount = 3000;
        [Space]

        public GameObject prefab, holder;

        public static string[] words;

        public int seed;
    
        public static float siblingDist = 4500;
        public static float scale = 100;

        private void Awake()
        {
            World.SetScene(Scenes.Galaxy);
        }

        private void Start()
        {
            systems = null;
            Init();
            DrawsSystems();
        }

        public static void GetWords()
        {
            if (words == null)
            {
                TextAsset mytxtData = (TextAsset) Resources.Load("words");
                var wrds = mytxtData.text;
                words = wrds.Split('/');
            }
        }

        public void Init()
        {
            World.SetScene(Scenes.Galaxy);
            GetWords();
            LoadSystems();
        }

        public static bool LoadSystems()
        {
            if (systems == null)
            {
                if (File.Exists(PlayerDataManager.GalaxyFile))
                {
                    try
                    {
                        var saved = JsonConvert.DeserializeObject<SavedGalaxy>(File.ReadAllText(PlayerDataManager.GalaxyFile));

                        if (saved.version == Application.version.ToString())
                        {
                            systems = saved.systems;
                            return true;
                        }
                        else
                        {
                            File.Delete(PlayerDataManager.GalaxyFile);
                            Directory.Delete(PlayerDataManager.CacheSystemsFolder, true);
                            ThrowLoadError($"Your game version [{Application.version}], galaxy version [{saved.version}]. Generate galaxy manually.");
                        }
                    }
                    catch (Exception e)
                    {
                        Directory.Move(PlayerDataManager.GlobalFolder, PlayerDataManager.PlayerFolder + "/Global Error Save " + DateTime.Now.ToString("dd-mm-yyyy-hh-mm-ss"));
                        Directory.Delete(PlayerDataManager.CacheSystemsFolder, true);
                        ThrowLoadError($"Loading galaxy error, your corrupted save moved to Saves/Player/Global Error Save");
                    }
                }
            }
            else
            {
                return true;
            }

            if (World.Scene == Scenes.Galaxy)
                World.LoadLevel(Scenes.Init);
        
            return false;
        }

        public static void ThrowLoadError(string text)
        {
            PlayerPrefs.SetString("Error", text);
            Destroy(PlayerDataManager.Instance.gameObject);
            PlayerDataManager.Instance = null;
            World.LoadLevel(Scenes.Init);
        }


        public void DrawsSystems()
        {
            foreach (var sys in systems)
            {
                if (sys.Value.sibligs.Count != 0)
                {
                    var point = Instantiate(prefab, holder.transform);
                    point.transform.position = sys.Value.position.ToVector() / scale;
                    var gpoint = point.GetComponent<GalaxyPoint>();
                    gpoint.solarSystem = sys.Value;
                }
            }
        }

        public static DVector GetSpawnPos(System.Random rnd)
        {
            bool canSpawn = false;
            var pos = new DVector(NextDecimal(rnd, -maxRadius, maxRadius), NextDecimal(rnd, minY, maxY), NextDecimal(rnd, -maxRadius, maxRadius));
            while (!canSpawn)
            {
                pos = new DVector(NextDecimal(rnd, -maxRadius, maxRadius), NextDecimal(rnd, minY, maxY), NextDecimal(rnd, -maxRadius, maxRadius));
                bool allOK = true;
                foreach (var item in systems)
                {
                    if (pos.Dist(item.Value.position) < minSystemsCount || pos.Dist(new DVector(0,0,0)) > maxRadius || pos.Dist(new DVector(0, 0, 0)) < minRadius )
                    {
                        allOK = false;
                        break;
                    }
                }
                if (allOK)
                {
                    canSpawn = true;
                    break;
                }
            }

            return pos;
        }

        public static SolarSystem GetBaseSystem(System.Random rnd)
        {
            var system = new SolarSystem();

            var pos = GetSpawnPos(rnd);
            var rot = new DVector(NextDecimal(rnd, 0, 360), NextDecimal(rnd, 0, 360), NextDecimal(rnd, 0, 360));


            system.rotation = rot;
            system.position = pos;

            return system;
        }
        
        public static void GetSiblings(SolarSystem system)
        {
            foreach (var sys in systems)
            {
                if (sys.Value.position.Dist(system.position) < siblingDist)
                {
                    var curr = new NeighbourSolarSytem() {position = system.position, solarName = system.name};
                    if (!sys.Value.sibligs.Contains(curr))
                    {
                        sys.Value.sibligs.Add(curr);
                    }

                    system.sibligs.Add(new NeighbourSolarSytem()
                        {position = sys.Value.position, solarName = sys.Value.name});
                }
            }
        }

        public static void AddToGalaxy(SolarSystem system)
        {
            if (!systems.ContainsKey(system.name))
            {
                systems.Add(system.name, system);
            }
        }

        public static List<OrbitStation> GetStaions(SolarSystem system)
        {
            var pos = system.position;
            var rnd = new System.Random((int) (pos.x + pos.y + pos.z));

            var starsCount = rnd.Next(1, 4);
            var planetsCount = rnd.Next(1, World.maxPlanetsCount * starsCount);
            var basesCount = rnd.Next(0, planetsCount);

            return SolarSystemGenerator.GenerateOrbitStations(basesCount, system.name, pos);
        }
        public static IEnumerator GenerateGalaxy(int seed)
        {
            GetWords();
            PlayerDataManager.GenerateProgress = 0;

            systems = new Dictionary<string, SolarSystem>();
        
            var rnd = new System.Random(seed);
        
            var systemsCount = rnd.Next(minSystemsCount, maxSystemsCount);

            for (int i = 0; i < systemsCount; i++)
            {
                var system = GetBaseSystem(rnd);
                system.stars.Add(SolarSystemGenerator.GenStars(1, system.name, system.position)[0]);
                system.SetName();
                system.stations = GetStaions(system);
                GetSiblings(system);
                AddToGalaxy(system);
            
                if (i % 5 == 0)
                {
                    PlayerDataManager.GenerateProgress = i / (float)systemsCount;
                    yield return null;
                }
            }
            SaveGalaxy();
        
            yield break;
        }

    
        public class SavedGalaxy
        {
            public Dictionary<string, SolarSystem> systems = new Dictionary<string, SolarSystem>();
            public string version = "";
        
            public SavedGalaxy(){}
        
        }
        public static void SaveGalaxy()
        {
            var galaxy = new SavedGalaxy() {systems = systems, version = Application.version};
            File.WriteAllText(PlayerDataManager.GalaxyFile, JsonConvert.SerializeObject(galaxy, Formatting.None));
            PlayerDataManager.GenerateProgress = 1f;
        }

        public static float NextDecimal(System.Random rnd, float min, float max)
        {
            return min + (max - min) * (float)rnd.NextDouble();
        }
    
    }
}
