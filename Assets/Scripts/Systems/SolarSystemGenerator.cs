using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class SavedSolarSystem
{
    public string systemName;
    public Vector3 playerPos;
    public Vector3 worldPos;
}

[System.Serializable]
public class SavedSolarSystemLocation
{
    public Vector3 playerPos;
    public bool startEnter;

}

public class SolarSystemGenerator : MonoBehaviour
{
    public GameObject sunPrefab, planetPrefab, stationPointPrefab, player, systemPoint;
    public static List<WorldSpaceObject> objects = new List<WorldSpaceObject>();
    public static List<WorldSpaceObject> suns = new List<WorldSpaceObject>();
    static SavedSolarSystem savedSolarSystem;

    public static Vector3 startPoint = Vector3.zero;

    public static void SaveSystem(bool isFirstInLocation = true)
    {
        File.WriteAllText(GetSystemFileName(), JsonConvert.SerializeObject(PlayerDataManager.CurrentSolarSystem));


        var system = new SavedSolarSystem();
        system.systemName = Path.GetFileNameWithoutExtension(GetSystemFileName());
        system.playerPos = Player.inst.transform.position;
        system.worldPos = objects[0].transform.parent.transform.position;

        File.WriteAllText(PlayerDataManager.CurrentSystemFile,
            JsonConvert.SerializeObject(system,
                new JsonSerializerSettings() {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
    }

    public void Load()
    {
        savedSolarSystem =
            JsonConvert.DeserializeObject<SavedSolarSystem>(File.ReadAllText(PlayerDataManager.CurrentSystemFile));
    }

    public void InitSystem()
    {
        if (PlayerDataManager.CurrentSolarSystem != null) //Generate And Save
        {
            if (!File.Exists(GetSystemFileName()))
            {
                PlayerDataManager.CurrentSolarSystem = Generate(PlayerDataManager.CurrentSolarSystem, planetPrefab);
            }
            else
            {
                if (File.Exists(GetSystemFileName()))
                    PlayerDataManager.CurrentSolarSystem =
                        JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(GetSystemFileName()));
            }
        }
        else
        {
            if (File.Exists(PlayerDataManager.CurrentSystemFile))
            {
                Load();
                var file = PlayerDataManager.CacheSystemsFolder + "/" + savedSolarSystem.systemName + ".solar";
                if (File.Exists(file))
                {
                    PlayerDataManager.CurrentSolarSystem =
                        JsonConvert.DeserializeObject<SolarSystem>(File.ReadAllText(file));
                }
                else
                {
                    print(1);
                    World.LoadLevel(Scenes.Init);
                }
            }
            else
            {                   
                print(2);
                World.LoadLevel(Scenes.Init);
            }
        }
    }

    public void CreateSystem()
    {
        if (PlayerDataManager.CurrentSolarSystem != null)
        {
            if (FindObjectOfType<Player>() == null)
            {
                Instantiate(player.gameObject).GetComponent<Player>().Init();
            }

            Player.inst.transform.parent = transform;
            Player.inst.transform.parent = null;
            Player.inst.transform.position = Vector3.zero;
            DrawAll(PlayerDataManager.CurrentSolarSystem, transform, sunPrefab, planetPrefab, stationPointPrefab,
                systemPoint, scale, savedSolarSystem == null);
            if (savedSolarSystem != null)
            {
                transform.position = savedSolarSystem.worldPos;
                Player.inst.transform.position = savedSolarSystem.playerPos;
            }
            else
            {
                startPoint = Player.inst.transform.position;
            }

            SaveSystem();
        }
    }

    private void Awake()
    {
        World.SetScene(Scenes.System);
    }

    private void OnEnable()
    {
        savedSolarSystem = null;


        InitSystem();
        CreateSystem();
    }

    public static string GetSystemFileName()
    {
        return PlayerDataManager.CacheSystemsFolder + "/" + PlayerDataManager.CurrentSolarSystem.name + "." +
               PlayerDataManager.CurrentSolarSystem.position.Log() + ".solar";
    }

    public static float scale = 15;

    public static SolarSystem Generate(SolarSystem solarSystem, GameObject planetPrefab)
    {
        var system = solarSystem;
        var pos = solarSystem.position;
        var rnd = new System.Random((int) (pos.x + pos.y + pos.z));

        system.stars = new List<Star>();
        var starsCount = rnd.Next(1, 4);

        var starTypes = System.Enum.GetNames(typeof(Star.StarType)).Length;

        for (int i = 0; i < starsCount; i++)
        {
            Star.StarType type = Star.StarType.M;
            DVector spos = new DVector();

            if (i == 0)
            {
                float xCoord = (float) pos.x / (float) GalaxyGenerator.maxRadius * 20f;
                float yCoord = (float) pos.y / (float) GalaxyGenerator.maxRadius * 20f;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                type = (Star.StarType) System.Math.Round(starTypes * sample);

            }
            else
            {
                type = (Star.StarType) rnd.Next(1, starTypes);
            }

            var star = new Star(type, rnd);

            star.position = spos;
            star.name = system.name.Split(' ')[0] + " " + (i != 0 ? i.ToString() : "");
            system.stars.Add(star);
        }


        var masses = system.stars.OrderBy(x => x.mass).ToList();
        for (int i = 1; i < starsCount; i++)
        {

            var spos = (masses[i - 1].position +
                        new DVector(0, 0, (masses[i - 1].radius * masses[i - 1].radius) + masses[i].radius) +
                        new DVector(0, 0, rnd.Next(15, 40) * i));
            masses[i].position = spos;
        }

        var planetsCount = rnd.Next(1, 5 * starsCount);

        for (int i = 0; i < planetsCount; i++)
        {
            DVector pPos = new DVector();
            if (i == 0)
            {
                var mostDist = system.stars.OrderBy(x => x.position.Dist(new DVector())).ToList();
                mostDist.Reverse();
                pPos = mostDist[0].position + new DVector(0, 0, rnd.Next(40, 100));
            }
            else
            {
                pPos = system.planets[i - 1].position + new DVector(0, 0, rnd.Next(20, 200));
            }

            var planet = new Planet(rnd, system.stars[system.stars.Count - 1], pPos);

            planet.name = system.name.Split(' ')[0] + " O" + (i + 1);
            var sattelites = rnd.Next(0, 3);

            DVector sPos = pPos;
            bool haveBase = false;
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
                    sattelite.textureID = rnd.Next(0, planetPrefab.GetComponent<PlanetTexture>().GetLen());
                    planet.sattelites.Add(sattelite);
                }
                else
                {
                    if (!haveBase)
                    {
                        sPos += new DVector(0, 0, planet.radius * 2m * rnd.Next(1, 3));

                        var station = new OrbitStation();
                        station.position = sPos;
                        station.rotation = new DVector(rnd.Next(0, 360), rnd.Next(0, 360), rnd.Next(0, 360));
                        station.name = system.name.Split(' ')[0] + " O" + (i + 1) + " " + (j + 1) + " Orbital";
                        planet.stations.Add(station);

                        haveBase = true;
                    }
                }
            }

            system.planets.Add(planet);
        }

        return system;
    }

    public static void DrawAll(SolarSystem system, Transform transform, GameObject sunPrefab, GameObject planetPrefab,
        GameObject stationPointPrefab, GameObject systemPoint, float _scale, bool setPos = true)
    {
        suns = new List<WorldSpaceObject>();
        objects = new List<WorldSpaceObject>();
        var pos = system.position;
        var rnd = new System.Random((int) (pos.x + pos.y + pos.z));
        Vector3 center = new Vector3(0, 0, 0);
        foreach (var star in PlayerDataManager.CurrentSolarSystem.stars)
        {
            center += star.position.ToVector() * _scale;
        }

        center /= PlayerDataManager.CurrentSolarSystem.stars.Count;

        var masses = PlayerDataManager.CurrentSolarSystem.stars.OrderBy(x => x.mass).ToList();
        masses.Reverse();

        for (int i = 0; i < masses.Count; i++)
        {
            center = Vector3.Lerp(center, masses[i].position.ToVector() * _scale,
                (float) masses[i].mass / (float) masses[0].mass);
        }

        GameObject attractor = new GameObject("Attractor");
        attractor.transform.position = center;
        attractor.transform.parent = transform;

        int id = 1;
        foreach (var item in PlayerDataManager.CurrentSolarSystem.stars)
        {
            var sun = Instantiate(sunPrefab, transform);

            sun.transform.name = item.name;
            sun.transform.position = item.position.ToVector() * _scale;
            sun.transform.localScale *= (float) item.radius * _scale;
        
            var renderer = sun.GetComponent<Renderer>();
            renderer.material.color = item.GetColor();
            renderer.material.SetColor("_MainColor", item.GetColor());
            renderer.material.SetFloat("_MainEmission", ((int)item.starType+1) * 50);
            
            var rotate = sun.GetComponent<RotateAround>();
            rotate.InitOrbit(attractor.transform, (float) rnd.NextDouble() * 0.01f, objects.Count);
            
            
            suns.Add(sun.GetComponent<WorldSpaceObject>());
            sun.GetComponentInChildren<Light>().color = item.GetColor();
            objects.Add(sun.GetComponent<WorldSpaceObject>());
            id++;
        }


        foreach (var item in PlayerDataManager.CurrentSolarSystem.planets)
        {
            var planet = Instantiate(planetPrefab, transform);
            planet.transform.name = item.name;
            planet.transform.position = item.position.ToVector() * _scale;
            planet.transform.localScale *= (float) item.radius * _scale;

            var rotate = planet.GetComponent<RotateAround>();
            rotate.InitOrbit(attractor.transform, (float) rnd.NextDouble() * 0.001f, objects.Count);
            
            objects.Add(planet.GetComponent<WorldSpaceObject>());

            for (int i = 0; i < item.sattelites.Count; i++)
            {
                var n = SpawnSattelite(planetPrefab, item.sattelites[i], planet.transform, _scale);
                objects.Add(n);
                var t = n.GetComponent<PlanetTexture>();
                t.SetTexture(item.sattelites[i].textureID);
            }

            for (int i = 0; i < item.stations.Count; i++)
            {
                objects.Add(SpawnSattelite(stationPointPrefab, item.stations[i], planet.transform, _scale));
            }
        }

        if (setPos)
        {
            FindObjectOfType<Player>().transform.position = new Vector3(0,
                (float) (masses[0].radius * rnd.Next(2, 6)) * _scale, (float) (masses[0].radius * 5) * _scale);
            FindObjectOfType<Player>().transform.LookAt(objects[0].transform);
        }
        else
        {
            startPoint = new Vector3(0, (float) (masses[0].radius * rnd.Next(2, 6)) * _scale,
                (float) (masses[0].radius * 5) * _scale);
        }

        var systemsParent = new GameObject("SystemsHolder");
        systemsParent.AddComponent<PosToPlayerPos>();
        foreach (var sibling in system.sibligs)
        {
            var syspoint = Instantiate(systemPoint, system.position.ToVector() - sibling.position.ToVector(),
                Quaternion.identity, systemsParent.transform);
            syspoint.transform.name = sibling.solarName + " S";
            syspoint.GetComponent<SolarSystemPoint>().systemName = sibling.solarName;
        }
    }

    public static WorldSpaceObject SpawnSattelite(GameObject prefab, SpaceObject item, Transform planet, float _scale)
    {
        var orbital = Instantiate(prefab);
        orbital.transform.name = item.name;
        orbital.transform.position = item.position.ToVector() * _scale;
        orbital.transform.localScale *= (float) item.radius * _scale;

        if (orbital.transform.localScale == Vector3.zero)
        {
            orbital.transform.localScale = Vector3.one;
        }

        orbital.transform.LookAt(planet.transform);
        var rotate = orbital.GetComponent<RotateAround>();
        rotate.InitOrbit(planet.transform, 0, 0, item.rotation.ToVector());
        
        orbital.transform.parent = planet.transform;
        return orbital.GetComponent<WorldSpaceObject>();
    }


    public static void DeleteSystemFile()
    {
        File.Delete(PlayerDataManager.CurrentSystemFile);
        PlayerDataManager.CurrentSolarSystem = null;
    }
}
