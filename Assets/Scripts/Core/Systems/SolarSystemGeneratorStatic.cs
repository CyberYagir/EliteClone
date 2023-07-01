using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Core.Inject.FoldersManagerService;
using Core.Core.Inject.GlobalDataService;
using Core.Galaxy;
using Core.Game;
using Core.PlayerScripts;
using Newtonsoft.Json;
using UnityEngine;
using Random = System.Random;

namespace Core.Systems
{
    public partial class SolarSystemGenerator
    {
        public static List<WorldSpaceObject> objects = new List<WorldSpaceObject>();
        public static List<WorldSpaceObject> suns = new List<WorldSpaceObject>();
        public static PlanetTextures planetTextures;
        public static Vector3 startPoint = Vector3.zero;
        public static float scale = 15;
        private static SavedSolarSystem savedSolarSystem;

        public static void SaveSystem(SolarSystemService solarSystemService, FolderManagerService folderManagerService)
        {
            var system = new SavedSolarSystem();
            system.systemName = Path.GetFileNameWithoutExtension(GetSystemFileName());

            if (Player.inst)
            {
                system.playerPos = Player.inst.transform.position;
            }

            if (objects.Count != 0)
            {
                system.worldPos = objects[0].transform.root.position;
            }

            File.WriteAllText(GetSystemFileName(), JsonConvert.SerializeObject(solarSystemService.CurrentSolarSystem));
            File.WriteAllText(folderManagerService.CurrentSystemFile, JsonConvert.SerializeObject(system, new JsonSerializerSettings {ReferenceLoopHandling = ReferenceLoopHandling.Ignore}));
        }

        public static SavedSolarSystem Load(FolderManagerService folderManagerService)
        {
            savedSolarSystem = JsonConvert.DeserializeObject<SavedSolarSystem>(File.ReadAllText(folderManagerService.CurrentSystemFile));
            return savedSolarSystem;
        }

        public static string GetSystemFileName(SolarSystemService solarSystemService, FolderManagerService folderManagerService)
        {
            return folderManagerService.CacheSystemsFolder + "/" + solarSystemService.CurrentSolarSystem.name + "." + solarSystemService.CurrentSolarSystem.position.Log() + ".solar";
        }

        public static string GetSystemName(SolarSystemService solarSystemService)
        {
            if (solarSystemService.CurrentSolarSystem != null)
            {
                return solarSystemService.CurrentSolarSystem.name + "." + solarSystemService.CurrentSolarSystem.position.Log();
            }

            return "";
        }

        public static void GetPlanetTextures()
        {
            if (planetTextures == null)
            {
                planetTextures = Resources.LoadAll<PlanetTextures>("")[0];
            }
        }

        public static List<Star> GenStars(int starsCount, string systemName, DVector pos)
        {
            List<Star> stars = new List<Star>();
            Random rnd = new Random((int) (pos.x + pos.y + pos.z));
            var fullStarsCount = GetStarsCount(pos);
            var starTypes = Enum.GetNames(typeof(Star.StarType)).Length;
            for (int i = 0; i < starsCount; i++)
            {
                Star.StarType type = Star.StarType.M;
                DVector spos = new DVector();

                if (i == 0)
                {
                    float xCoord = pos.x / GalaxyGenerator.maxRadius * 20f;
                    float yCoord = pos.y / GalaxyGenerator.maxRadius * 20f;
                    float sample = Mathf.PerlinNoise(xCoord, yCoord);
                    type = (Star.StarType) Math.Round(starTypes * sample);
                }
                else
                {
                    type = (Star.StarType) rnd.Next(1, starTypes);
                }

                if (fullStarsCount == 1)
                {
                    if (rnd.Next(0, 100) < 20)
                    {
                        type = Star.StarType.Hole;
                    }
                }

                var star = new Star(type, rnd);
                star.position = spos;
                if (systemName != null)
                {
                    star.name = systemName.Split(' ')[0] + " " + (i != 0 ? i.ToString() : "");
                }

                stars.Add(star);
            }

            return stars;
        }

        public static int GetStarsCount(DVector pos)
        {
            var rnd = new Random((int) (pos.x + pos.y + pos.z));
            return rnd.Next(1, 4);
        }

        public static SolarSystem Generate(SolarSystem solarSystem)
        {
            GetPlanetTextures();
            var system = solarSystem;
            var pos = solarSystem.position;
            var rnd = new Random((int) (pos.x + pos.y + pos.z));
            var starsCount = GetStarsCount(system.position);
            var planetsCount = rnd.Next(1, World.maxPlanetsCount * starsCount);
            var basesCount = rnd.Next(0, planetsCount + 1);
            var beltsCount = rnd.Next(0, 4);

            system.stars = GenStars(starsCount, system.name, system.position);
            system.stations = GenerateOrbitStations(basesCount, system.name, system.position);
            system.belts = GenerateBelts(beltsCount, system.name, system.position);

            var masses = system.stars.OrderBy(x => x.mass).ToList();
            for (int i = 1; i < starsCount; i++)
            {
                var spos = (masses[i - 1].position +
                            new DVector(0, 0, (masses[i - 1].radius * masses[i - 1].radius) + (masses[i].radius + rnd.Next(15, 40) * i)));
                masses[i].position = spos;
            }

            int usesBases = 0;
            for (int i = 0; i < planetsCount; i++)
            {
                DVector pPos = new DVector();
                if (i == 0)
                {
                    var mostDist = system.stars.OrderBy(x => x.position.Dist(new DVector())).ToList();
                    mostDist.Reverse();
                    pPos = mostDist[0].position + new DVector(0, 0, rnd.Next(40, 100) + mostDist[0].radius);
                    if (mostDist[0].starType == Star.StarType.Hole)
                    {
                        pPos += new DVector(0, 0, mostDist[0].radius);
                    }
                }
                else
                {
                    pPos = system.planets[i - 1].position + new DVector(0, 0, rnd.Next(20, 200));
                }

                var planet = new Planet(rnd, pPos);

                planet.textureID = rnd.Next(0, planetTextures.textures.Count);
                planet.name = system.name.Split(' ')[0] + " O" + (i + 1);
                var sattelites = rnd.Next(0, 3);

                DVector sPos = pPos;
                bool haveBase = false;
                for (int j = 0; j < sattelites; j++)
                {
                    if (haveBase || usesBases >= basesCount)
                    {
                        sPos += new DVector(0, 0, planet.radius * 1.5f * rnd.Next(1, 3));
                        var sattelite = new Planet(rnd, sPos, true);
                        sattelite.position = sPos;
                        sattelite.rotation = new DVector(rnd.Next(0, 360), rnd.Next(0, 360), rnd.Next(0, 360));
                        sattelite.mass *= 0.1f;
                        sattelite.radius *= 0.1f;
                        sattelite.name = system.name.Split(' ')[0] + " O" + (i + 1) + " " + (j + 1);

                        var textureID = rnd.Next(0, planetTextures.textures.Count);
                        while (planetTextures.textures[textureID].type == Planet.PlanetType.Gas)
                        {
                            textureID = rnd.Next(0, planetTextures.textures.Count);
                        }

                        sattelite.textureID = textureID;
                        planet.sattelites.Add(sattelite);
                    }
                    else
                    {
                        sPos += new DVector(0, 0, planet.radius * 2f * rnd.Next(1, 3));
                        system.stations[usesBases].position = sPos;
                        planet.stations.Add(system.stations[usesBases]);
                        usesBases++;
                        haveBase = true;
                    }
                }

                system.planets.Add(planet);
            }

            if (usesBases < basesCount) //если не все станции заспавнились
            {
                for (int i = 0; i < planetsCount; i++)
                {
                    if (system.planets[i].stations.Count == 0)
                    {
                        var nPos = system.planets[i].position + new DVector(0, 0, system.planets[i].radius * 2f * rnd.Next(1, 3));
                        system.stations[usesBases].position = nPos;
                        system.planets[i].stations.Add(system.stations[usesBases]);
                        usesBases++;
                        if (usesBases >= basesCount)
                        {
                            break;
                        }
                    }
                }
            }


            for (int i = 0; i < beltsCount; i++)
            {
                var bpos = new DVector(0, 0, system.planets[rnd.Next(0, system.planets.Count)].position.z);
                bpos += new DVector(0, 0, rnd.Next(15, 30));
                system.belts[i].position = bpos;
            }

            return system;
        }

        private static List<Belt> GenerateBelts(int beltsCount, string systemName, DVector pos)
        {
            List<Belt> belts = new List<Belt>();
            Random rnd = new Random((int) (pos.x + pos.y + pos.z));

            for (int i = 0; i < beltsCount; i++)
            {
                var belt = new Belt(rnd)
                {
                    name = systemName.Split(' ')[0] + " Belt #" + i
                };
                belts.Add(belt);
            }

            return belts;
        }

        public static List<OrbitStation> GenerateOrbitStations(int stationsCount, string systemName, DVector pos)
        {
            List<OrbitStation> stations = new List<OrbitStation>();
            Random rnd = new Random((int) (pos.x + pos.y + pos.z));
            for (int i = 0; i < stationsCount; i++)
            {
                var station = new OrbitStation();
                station.name = systemName.Split(' ')[0] + " Orbital #" + i;
                station.rotation = new DVector(rnd.Next(0, 360), rnd.Next(0, 360), rnd.Next(0, 360));
                stations.Add(station);
            }

            return stations;
        }

        
        public static WorldSpaceObject SpawnSattelite(GameObject prefab, SpaceObject item, Transform planet, float _scale)
        {
            var orbital = Instantiate(prefab);
            orbital.transform.name = item.name;
            orbital.transform.position = item.position.ToVector() * _scale;
            orbital.transform.localScale *= item.radius * _scale;

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
        
        public static void DrawAll(SolarSystemService solarSystemService, SolarSystem system, Transform transform, GameObject sunPrefab, GameObject planetPrefab,
            GameObject stationPointPrefab, GameObject systemPoint, GameObject beltPoint, float _scale, bool setPos = true)
        {
            transform.position = Vector3.zero;
            suns = new List<WorldSpaceObject>();
            objects = new List<WorldSpaceObject>();
            var pos = system.position;
            var rnd = new Random((int) (pos.x + pos.y + pos.z));
            Vector3 center = new Vector3(0, 0, 0);
            foreach (var star in solarSystemService.CurrentSolarSystem.stars)
            {
                center += star.position.ToVector() * _scale;
            }

            center /= solarSystemService.CurrentSolarSystem.stars.Count;

            var masses = solarSystemService.CurrentSolarSystem.stars.OrderBy(x => x.mass).ToList();
            masses.Reverse();

            for (int i = 0; i < masses.Count; i++)
            {
                center = Vector3.Lerp(center, masses[i].position.ToVector() * _scale,
                    masses[i].mass / masses[0].mass);
            }

            GameObject attractor = new GameObject("Attractor");
            attractor.transform.position = center;
            attractor.transform.parent = transform;

            int id = 1;
            foreach (var item in solarSystemService.CurrentSolarSystem.stars)
            {
                var sun = Instantiate(sunPrefab, transform);

                sun.transform.name = item.name;
                sun.transform.position = item.position.ToVector() * _scale;
                sun.transform.localScale *= item.radius * _scale;

                sun.GetComponent<SunTexture>().SetMaterials(item, ((int) item.starType + 1) * 50);

                var rotate = sun.GetComponent<RotateAround>();
                rotate.InitOrbit(attractor.transform, (float) rnd.NextDouble() * 0.01f, objects.Count);


                suns.Add(sun.GetComponent<WorldSpaceObject>());
                sun.GetComponentInChildren<Light>().color = item.GetColor();
                objects.Add(sun.GetComponent<WorldSpaceObject>());
                id++;
            }


            foreach (var item in solarSystemService.CurrentSolarSystem.planets)
            {
                var planet = Instantiate(planetPrefab, transform);
                planet.transform.name = item.name;
                planet.transform.position = item.position.ToVector() * _scale;
                planet.transform.localScale *= item.radius * _scale;

                var rotate = planet.GetComponent<RotateAround>();
                rotate.InitOrbit(attractor.transform, (float) rnd.NextDouble() * 0.001f, objects.Count);
                rotate.OnRotate += delegate { rotate.transform.RotateAround(attractor.transform.position, new Vector3(rnd.Next(-1, 2), rnd.Next(-1, 2), rnd.Next(-1, 2)), rnd.Next(-3, 3)); };

                planet.GetComponent<PlanetTexture>().SetTexture(item.textureID);

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

            foreach (var item in solarSystemService.CurrentSolarSystem.belts)
            {
                var belt = Instantiate(beltPoint, transform);
                belt.transform.name = item.name;
                belt.transform.position = item.position.ToVector() * _scale;
                belt.transform.RotateAround(center, Vector3.up, rnd.Next(0, 360));

                objects.Add(belt.GetComponent<WorldSpaceObject>());
            }


            startPoint = center + Vector3.one * (masses[0].radius * _scale * 4);
            if (setPos)
            {
                //print("SetPos");
                FindObjectOfType<Player>().transform.position = startPoint;
                FindObjectOfType<Player>().transform.LookAt(objects[0].transform);
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


        public static void DeleteSystemFile(SolarSystemService solarSystemService, FolderManagerService folderManagerService)
        {
            File.Delete(folderManagerService.CurrentSystemFile);
            solarSystemService.SetSolarSystem(null);
        }
    }
}