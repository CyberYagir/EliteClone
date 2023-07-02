using System.Collections.Generic;
using System.IO;
using Core.Galaxy;
using Newtonsoft.Json;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    public static class SolarSystemShipsStaticBuilder
    {
        public static Dictionary<string, List<SolarSystemShips.HumanShipDead>> deadList { get; private set; } = new Dictionary<string, List<SolarSystemShips.HumanShipDead>>();

        public static void LoadDeads()
        {
            if (File.Exists(PlayerDataManager.Instance.FSHandler.DeadsNpcFile) && deadList.Count == 0)
            {
                deadList = JsonConvert.DeserializeObject<Dictionary<string, List<SolarSystemShips.HumanShipDead>>>(File.ReadAllText(PlayerDataManager.Instance.FSHandler.DeadsNpcFile));
            }
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

        public static SolarSystemShips.ShipsData InitShipsPoses(string systemName)
        {
            var shipsData = new SolarSystemShips.ShipsData();
            var allships = new List<SolarSystemShips.HumanShip>();
            var ships = new List<SolarSystemShips.HumanShip>();
            var locations = new List<SolarSystemShips.LocationHolder>();

            var system = GalaxyGenerator.systems[systemName];
            
            
            for (int i = 0; i < system.stations.Count; i++)
            {
                locations.Add(new SolarSystemShips.LocationHolder(system.stations[i].name));
            }

            for (int i = 0; i < system.belts.Count; i++)
            {
                locations.Add(new SolarSystemShips.LocationHolder(system.belts[i].name));
            }
            var seed = GetSeed(system.position.ToVector());
            var positionsRnd = new Random(seed + SaveLoadData.GetCurrentSaveSeed());
            var shipsGened = GetShips(system);
            
            
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

            shipsData.allships = allships;
            shipsData.ships = ships;
            shipsData.locations = locations;

            return shipsData;
        }

        public static List<SolarSystemShips.HumanShip> GetShips(SolarSystem solar)
        {
            var ships = new List<SolarSystemShips.HumanShip>();
            var count = GetShipsCount(solar.position.ToVector(), solar.stations.Count);
            if (solar.stations.Count + solar.belts.Count < 2)
            {
                count = Mathf.Clamp(count, 2, 10);
            }
            
            var seed = GetSeed(solar.position.ToVector());
            var rnd = new Random(seed);
            
            
            for (int i = 0; i < count; i++)
            {
                var ship = GenerateHuman(rnd, seed, i);
                ships.Add(ship);
            }

            return ships;
        }

        public static SolarSystemShips.HumanShip GenerateHuman(System.Random rnd, int seed, int i)
        {
            return new SolarSystemShips.HumanShip(rnd, rnd.Next(0,4), seed + i);
        }
    }
}