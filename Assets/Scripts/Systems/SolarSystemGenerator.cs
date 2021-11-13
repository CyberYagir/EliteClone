using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public static class World
{
    public static readonly float unitSize = 100;
}

public class SolarSystemGenerator : MonoBehaviour
{
    public GameObject sunPrefab, planetPrefab;

    public static List<WorldSpaceObject> objects = new List<WorldSpaceObject>();

    private void Awake()
    {
        if (PlayerDataManager.currentSolarSystem != null)
        {
            if (PlayerDataManager.saveSystems)
            {
                if (!File.Exists(GetSystemFileName()))
                {
                    Generate();
                    File.WriteAllText(GetSystemFileName(), JsonConvert.SerializeObject(PlayerDataManager.currentSolarSystem));
                    File.WriteAllText(PlayerDataManager.currentSystemFile, JsonConvert.SerializeObject(Path.GetFileNameWithoutExtension(GetSystemFileName())));
                }
                else
                {
                    PlayerDataManager.currentSolarSystem = JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(GetSystemFileName()));
                }
            }
            else
            {
                Generate();
            }
        }
        else
        {
            if (File.Exists(PlayerDataManager.currentSystemFile))
            {
                var file = PlayerDataManager.cacheSystemsFolder + "/" + JsonConvert.DeserializeObject<string>(File.ReadAllText(PlayerDataManager.currentSystemFile)) + ".solar";
                if (File.Exists(file))
                {
                    PlayerDataManager.currentSolarSystem = JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(file));
                }
                else
                {
                    Application.LoadLevel("Init");
                }
            }
            else
            {
                Application.LoadLevel("Init");
            }
        }
        if (PlayerDataManager.currentSolarSystem != null)
            DrawAll();
    }

    public string GetSystemFileName()
    {
        return PlayerDataManager.cacheSystemsFolder + "/" + PlayerDataManager.currentSolarSystem.name + "." + PlayerDataManager.currentSolarSystem.position.Log() + ".solar";
    }

    public const float scale = 15;

    public void Generate()
    {
        var pos = PlayerDataManager.currentSolarSystem.position;
        var rnd = new System.Random((int)(pos.x + pos.y + pos.z));
        var system = PlayerDataManager.currentSolarSystem;

        system.stars = new List<Star>();
        var starsCount = rnd.Next(1, 4);

        var starTypes = System.Enum.GetNames(typeof(Star.StarType)).Length;

        for (int i = 0; i < starsCount; i++)
        {
            Star.StarType type = Star.StarType.M;
            DVector spos = new DVector();

            if (i == 0)
            {
                float xCoord = (float)pos.x / (float)GalaxyGenerator.maxRadius * 20f;
                float yCoord = (float)pos.y / (float)GalaxyGenerator.maxRadius * 20f;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                type = (Star.StarType)System.Math.Round(starTypes * sample);

            }
            else
            {
                type = (Star.StarType)rnd.Next(1, starTypes);
            }
            var star = new Star(type, rnd);
            
            star.position = spos;
            star.name = system.name.Split(' ')[0] + " " + (i != 0 ? i.ToString() : "");
            system.stars.Add(star);
        }


        var masses = PlayerDataManager.currentSolarSystem.stars.OrderBy(x => x.mass).ToList();
        for (int i = 1; i < starsCount; i++)
        {

            var spos = (masses[i - 1].position + new DVector(0, 0, (masses[i - 1].radius * masses[i - 1].radius) + masses[i].radius) + new DVector(0, 0, rnd.Next(15, 40) * i));
            masses[i].position = spos;
        }

        var planetsCount = rnd.Next(1, 5 * starsCount);

        for (int i = 0; i < planetsCount; i++)
        {
            DVector pPos = new DVector();
            if (i == 0)
            {
                var mostDist = PlayerDataManager.currentSolarSystem.stars.OrderBy(x => x.position.Dist(new DVector())).ToList();
                mostDist.Reverse();
                pPos = mostDist[0].position + new DVector(0, 0, rnd.Next(40, 100));
            }
            else
            {
                pPos = system.planets[i - 1].position + new DVector(0, 0, rnd.Next(20, 200));
            }

            var planet = new Planet(rnd, PlayerDataManager.currentSolarSystem.stars[PlayerDataManager.currentSolarSystem.stars.Count - 1], pPos);

            planet.name = system.name.Split(' ')[0] + " O" + (i+1);
            var sattelites = rnd.Next(0, 3);

            DVector sPos = pPos;
            for (int j = 0; j < sattelites; j++)
            {
                var isBase = rnd.Next(0, 4);
                if (isBase <= 2)
                {
                    sPos += new DVector(0, 0, planet.radius * 2m * rnd.Next(1, 3));

                    var sattelite = new Planet(rnd, planet, sPos);
                    sattelite.position = sPos;
                    sattelite.rotation = new DVector(rnd.Next(0, 360), rnd.Next(0, 360), rnd.Next(0, 360));
                    sattelite.mass *= 0.1m;
                    sattelite.radius *= 0.1m;
                    sattelite.name = system.name.Split(' ')[0] + " O" + (i + 1) + " " + (j + 1);

                    planet.sattelites.Add(sattelite);
                }
            }
            system.planets.Add(planet);
        }

    }

    public void DrawAll()
    {
        objects = new List<WorldSpaceObject>();
        var pos = PlayerDataManager.currentSolarSystem.position;
        var rnd = new System.Random((int)(pos.x + pos.y + pos.z));
        Vector3 center = new Vector3(0,0,0);
        foreach (var star in PlayerDataManager.currentSolarSystem.stars)
        {
            center += star.position.toVector() * scale;
        }
        center /= PlayerDataManager.currentSolarSystem.stars.Count;

        var masses = PlayerDataManager.currentSolarSystem.stars.OrderBy(x => x.mass).ToList();
        masses.Reverse();

        for (int i = 0; i < masses.Count; i++)
        {
            center = Vector3.Lerp(center, masses[i].position.toVector() * scale, (float)masses[i].mass / (float)masses[0].mass);
        }

        GameObject attractor = new GameObject("Attractor");
        attractor.transform.position = center;

        int id = 1;
        foreach (var item in PlayerDataManager.currentSolarSystem.stars)
        {
            var sun = Instantiate(sunPrefab);
            sun.transform.name = item.name;
            sun.transform.position = item.position.toVector() * scale;
            sun.transform.localScale *= (float)item.radius * scale;

            sun.GetComponent<Renderer>().material.color = item.GetColor();
            sun.GetComponent<Renderer>().material.SetColor("_EmissiveColor", item.GetColor());
            sun.GetComponent<RotateAround>().point = attractor.transform;

            sun.GetComponent<RotateAround>().speed = (float)rnd.NextDouble() * 0.01f;


            sun.GetComponentInChildren<Light>().intensity = (99999999f * Mathf.Clamp01(((float)item.mass / (120f / scale))));
            sun.GetComponentInChildren<Light>().color = item.GetColor();
            sun.GetComponent<RotateAround>().orbitID = objects.Count;
            objects.Add(sun.GetComponent<WorldSpaceObject>());
            id++;
        }


        foreach (var item in PlayerDataManager.currentSolarSystem.planets)
        {
            var planet = Instantiate(planetPrefab);
            planet.transform.name = item.name;
            planet.transform.position = item.position.toVector() * scale;
            planet.transform.localScale *= (float)item.radius * scale;

            planet.GetComponent<RotateAround>().point = attractor.transform;
            planet.GetComponent<RotateAround>().orbitID = objects.Count;
            planet.GetComponent<RotateAround>().speed = (float)rnd.NextDouble() * 0.001f;
            objects.Add(planet.GetComponent<WorldSpaceObject>());

            for (int i = 0; i < item.sattelites.Count; i++)
            {

                var sattelite = Instantiate(planetPrefab);
                sattelite.transform.name = item.sattelites[i].name;
                sattelite.transform.position = item.sattelites[i].position.toVector() * scale;
                sattelite.transform.localScale *= (float)item.sattelites[i].radius * scale;
                sattelite.transform.LookAt(planet.transform);
                sattelite.GetComponent<RotateAround>().point = planet.transform;
                sattelite.GetComponent<RotateAround>().orbitRotation = item.sattelites[i].rotation.toVector();
                sattelite.transform.parent = planet.transform;
                objects.Add(sattelite.GetComponent<WorldSpaceObject>());
            }
        }

        FindObjectOfType<Player>().transform.position = new Vector3(0, (float)(masses[0].radius * rnd.Next(2, 6)) * scale, (float)(masses[0].radius * 5) * scale);
        FindObjectOfType<Player>().transform.LookAt(objects[0].transform);
    }
}
