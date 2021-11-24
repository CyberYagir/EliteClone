using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class GalaxyGenerator : MonoBehaviour
{
    public static Dictionary<string,SolarSystem> systems;
    public static decimal maxY = 5000, minY = -5000;
    public static decimal maxRadius = 100000, minRadius = 30000;
    public static int maxSystemsCount = 4000, minSystemsCount = 2000;
    public static float minBetweenGalaxiesDistance = 2;
    [Space]

    public GameObject prefab, holder;

    public static string[] words;

    public int seed;
    
    public static float siblingDist = 6000;
    public static float scale = 100;

    private void Start()
    {
        systems = null;
        Init();
        DrawsSystems();
    }

    public static void GetWords()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("words");
        var wrds = mytxtData.text;
        words = wrds.Split('/');
    }

    public void Init()
    {
        GetWords();
        LoadSystems();
    }

    public static void LoadSystems()
    {
        if (systems == null)
        {
            if (File.Exists(PlayerDataManager.galaxyFile))
            {
                systems = JsonConvert.DeserializeObject<Dictionary<string,SolarSystem>>(
                    File.ReadAllText(PlayerDataManager.galaxyFile));

                return;
            }
        }

        if (Application.loadedLevelName == "Galaxy")
            Application.LoadLevel("Init");
    }

    public void DrawsSystems()
    {
        foreach (var sys in systems)
        {
            var point = Instantiate(prefab, holder.transform);
            point.transform.position = sys.Value.position.toVector() / scale;
            var gpoint = point.GetComponent<GalaxyPoint>();
            gpoint.solarSystem = sys.Value;
        }
    }

    public static IEnumerator GenerateGalaxy(int seed)
    {
        if (words == null)
        {
            GetWords();
        }


        PlayerDataManager.generateProgress = 0;

        systems = new Dictionary<string, SolarSystem>();
        
        var rnd = new System.Random(seed);

        var systemsCount = rnd.Next(minSystemsCount, maxSystemsCount);

        for (int i = 0; i < systemsCount; i++)
        {
            var system = new SolarSystem();
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
                if (allOK == true)
                {
                    canSpawn = true;
                    break;
                }
            }
            var rot = new DVector(NextDecimal(rnd, 0, 360), NextDecimal(rnd, 0, 360), NextDecimal(rnd, 0, 360));
            system.rotation = rot;
            system.position = pos;

            var rnd1 = new System.Random((int)(pos.x + pos.y + pos.z));

            var starsCount = rnd1.Next(1, 3);

            var starTypes = System.Enum.GetNames(typeof(Star.StarType)).Length;

            float xCoord = (float)pos.x / (float)GalaxyGenerator.maxRadius * 20f;
            float yCoord = (float)pos.y / (float)GalaxyGenerator.maxRadius * 20f;
            float sample = Mathf.PerlinNoise(xCoord, yCoord);

            Star.StarType type = (Star.StarType)System.Math.Round(starTypes * sample);

            var newStar = new Star(type, rnd1);

            system.stars.Add(newStar);
            system.name = system.stars[0].name;

            foreach (var sys in systems)
            {
                if (sys.Value.position.Dist(system.position) < (decimal) siblingDist)
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

            if (!systems.ContainsKey(system.name))
            {
                systems.Add(system.name, system);
            }

            if (i % 5 == 0)
            {
                PlayerDataManager.generateProgress = i / (float)systemsCount;
                yield return null;
            }
        }
        File.WriteAllText(PlayerDataManager.galaxyFile, JsonConvert.SerializeObject(systems, Formatting.None));
        PlayerDataManager.generateProgress = 1f;

        yield break;
    }

    public static decimal NextDecimal(System.Random rnd, decimal min, decimal max)
    {
        return min + (max - min) * (decimal)rnd.NextDouble();
    }
}
