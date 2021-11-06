using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

[System.Serializable]
public struct DVector
{
    public decimal x, y,z;

    public DVector(decimal x, decimal y, decimal z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3 toVector()
    {
        return new Vector3((float)x, (float)y, (float)z);
    }
    public string Log()
    {
        return $"[{x.ToString("F2")},{y.ToString("F2")},{z.ToString("F2")}]";
    }

    public decimal Dist(DVector second)
    {
        return Sqrt(Pow(second.x - x, 2) + Pow(second.y - y, 2) + Pow(second.z - z, 2));
    }

    public decimal Pow(decimal num, int n)
    {
        decimal final = num;
        for (int i = 0; i < n-1; i++)
        {
            final *= num;
        }

        return final;
    }

    public decimal Sqrt(decimal x, decimal epsilon = 0.0M)
    {
        if (x < 0) throw new System.OverflowException("Cannot calculate square root from a negative number");

        decimal current = (decimal)System.Math.Sqrt((double)x), previous;
        do
        {
            previous = current;
            if (previous == 0.0M) return 0;
            current = (previous + x / previous) / 2;
        }
        while (System.Math.Abs(previous - current) > epsilon);
        return current;
    }
}
[System.Serializable]
public class SpaceObject
{
    public string name;
    public DVector position;
    public DVector rotation;
    public decimal mass, radius;

   

    public SpaceObject(string name, DVector position, DVector rotation, decimal mass, decimal radius)
    {
        this.name = name;
        this.position = position;
        this.rotation = rotation;
        this.mass = mass;
        this.radius = radius;
    }

    public SpaceObject(DVector position, DVector rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }

    public SpaceObject()
    {

    }
}
[System.Serializable]
public class Star: SpaceObject
{
    public enum StarType { M, K, G, F, A, B, O}
    public StarType starType;

    public Star(StarType starType, System.Random rnd)
    {
        name = GenerateName(rnd);
        this.starType = starType;
        switch (starType)
        {
            case StarType.M:
                mass = GalaxyGenerator.NextDecimal(rnd, 0.30m, 0.7m);
                radius = GalaxyGenerator.NextDecimal(rnd, 0.3m, 0.8m);
                break;
            case StarType.K:
                mass = GalaxyGenerator.NextDecimal(rnd, 0.8m, 1.3m);
                radius = GalaxyGenerator.NextDecimal(rnd, 0.9m, 1.1m);
                break;
            case StarType.G:
                mass = GalaxyGenerator.NextDecimal(rnd, 1.6m, 2.6m);
                radius = GalaxyGenerator.NextDecimal(rnd, 1.1m, 1.3m);
                break;
            case StarType.F:
                mass = GalaxyGenerator.NextDecimal(rnd, 3m, 16m);
                radius = GalaxyGenerator.NextDecimal(rnd, 1.3m, 2.5m);
                break;
            case StarType.A:
                mass = GalaxyGenerator.NextDecimal(rnd, 18m, 55m);
                radius = GalaxyGenerator.NextDecimal(rnd, 2.5m, 7m);
                break;
            case StarType.B:
                mass = GalaxyGenerator.NextDecimal(rnd, 18m, 40m);
                radius = GalaxyGenerator.NextDecimal(rnd, 7m, 15m);
                break;
            case StarType.O:
                mass = GalaxyGenerator.NextDecimal(rnd, 55m, 120m);
                radius = GalaxyGenerator.NextDecimal(rnd, 15m, 40m);
                break;
            default:
                mass = -1;
                radius = -1;
                break;
        }
    }

    public Star()
    {

    }

    public static string GenerateName(System.Random rnd)
    {
        var str = GalaxyGenerator.words[rnd.Next(0, GalaxyGenerator.words.Length)];

        if (str.Length == 1)
            str = char.ToUpper(str[0]).ToString();
        else
            str = (char.ToUpper(str[0]) + str.Substring(1));

        return str + " " + rnd.Next(0, 999999).ToString("0000000");
    }
}
[System.Serializable]
public class Planet: SpaceObject
{
    public List<SpaceObject> sattelites = new List<SpaceObject>();

    public enum GroundType { Gas, Stone};
    public enum AtmosphereType { None, NotDense, Dense, ExtraDense };
    public enum FluidsType { Yes, No};
    public enum OceansType { No, Small, Large, All};

    public GroundType ground;
    public AtmosphereType atmosphere;
    public FluidsType fluids;
    public OceansType oceans;

    public SpaceObject parent;

    public float temperature;

    

    public Planet()
    {

    }
}

[System.Serializable]
public class OrbitStation
{

}

[System.Serializable]

public class Belt : SpaceObject
{
    public enum ClasterType { Stones, Metals, Crystals, Mixed };

    public ClasterType claster;

    
}
[System.Serializable]
public class SolarSystem:SpaceObject
{
    public List<Star> stars = new List<Star>();
}

public class GalaxyGenerator : MonoBehaviour
{
    List<SolarSystem> systems = new List<SolarSystem>();
    public decimal maxY = 5000, minY = -5000;
    public decimal maxRadius = 100000, minRadius = 30000;
    public int maxSystemsCount, minSystemsCount;
    public float minBetweenGalaxiesDistance;
    [Space]

    public float maxPlanetRadius;

    public GameObject prefab;

    public static string[] words;

    public int seed;

    private void Start()
    {
        GenerateGalaxy(seed);
        DrawsSystems();
    }
    public void Init()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("words");
        var wrds =  mytxtData.text;
        words = wrds.Split('/');
    }

    public void DrawsSystems()
    {
        for (int i = 0; i < systems.Count; i++)
        {
            var point = Instantiate(prefab);
            point.transform.position = systems[i].position.toVector() / 100f;
            point.GetComponent<GalaxyPoint>().solarSystem = systems[i];
        }
    }

    public void GenerateGalaxy(int seed)
    {
        Init();
        var rnd = new System.Random(seed);

        var systemsCount = rnd.Next(minSystemsCount, maxSystemsCount);

        for (int i = 0; i < systemsCount; i++)
        {
            var system = new SolarSystem();
            system.name = Star.GenerateName(rnd);


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

            rnd = new System.Random((int)(pos.x + pos.y));

            var starsCount = rnd.Next(1, 3);

            var starTypes = System.Enum.GetNames(typeof(Star.StarType)).Length;

            float xCoord = (float)pos.x / (float)maxRadius * 20f;
            float yCoord = (float)pos.y / (float)maxRadius * 20f;
            float sample = Mathf.PerlinNoise(xCoord, yCoord);

            Star.StarType type = (Star.StarType)System.Math.Round(starTypes * sample);
            var newStar = new Star(type, rnd);
            system.stars.Add(newStar);

            systems.Add(system);
        }


        //File.WriteAllText("D:/1.json", JsonConvert.SerializeObject(systems, Formatting.None));
    }

    public DVector centerOfMass(List<Star> stars)
    {
        var totalX = 0m;
        var totalY = 0m;
        var totalZ = 0m;
        foreach (var s in stars)
        {
            totalX += s.position.x;
            totalY += s.position.y;
            totalZ += s.position.z;
        }
        var centerX = totalX / stars.Count;
        var centerY = totalY / stars.Count;
        var centerZ = totalZ / stars.Count;

        return new DVector(centerX, centerY, centerZ);
    }

    public static decimal NextDecimal(System.Random rnd, decimal min, decimal max)
    {
        return min + (max - min) * (decimal)rnd.NextDouble();
    }
}
