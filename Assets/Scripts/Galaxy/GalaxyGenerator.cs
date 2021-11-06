using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class GalaxyGenerator : MonoBehaviour
{
    List<SolarSystem> systems = new List<SolarSystem>();
    public decimal maxY = 5000, minY = -5000;
    public static decimal maxRadius = 100000, minRadius = 30000;
    public int maxSystemsCount, minSystemsCount;
    public float minBetweenGalaxiesDistance;
    [Space]

    public float maxPlanetRadius;

    public GameObject prefab, holder;

    public static string[] words;

    public int seed;

    private void Start()
    {
        Init();
        DrawsSystems();
    }
    public void Init()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("words");
        var wrds = mytxtData.text;
        words = wrds.Split('/');

        var path = Application.persistentDataPath + "/galaxy.json";
        if (File.Exists(path))
        {
            systems = JsonConvert.DeserializeObject<List<SolarSystem>>(File.ReadAllText(path));
        }
        else
        {
            GenerateGalaxy(seed);
            File.WriteAllText(path, JsonConvert.SerializeObject(systems, Formatting.None));
        }
    }

    public void DrawsSystems()
    {
        for (int i = 0; i < systems.Count; i++)
        {
            var point = Instantiate(prefab, holder.transform);
            point.transform.position = systems[i].position.toVector() / 100f;
            point.GetComponent<GalaxyPoint>().solarSystem = systems[i];
        }
    }

    public void GenerateGalaxy(int seed)
    {
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
                    if (pos.Dist(item.position) < minSystemsCount || pos.Dist(new DVector(0,0,0)) > maxRadius || pos.Dist(new DVector(0, 0, 0)) < minRadius )
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
            systems.Add(system);
        }
    }

    public static decimal NextDecimal(System.Random rnd, decimal min, decimal max)
    {
        return min + (max - min) * (decimal)rnd.NextDouble();
    }
}
