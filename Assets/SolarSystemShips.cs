using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SolarSystemShips : MonoBehaviour
{
    [System.Serializable]
    public class LocationHolder
    {
        public string locationName;
        public List<HumanShip> humans = new List<HumanShip>();

        public LocationHolder(string name)
        {
            locationName = name;
        }
    }

    [System.Serializable]
    public class HumanShip
    {
        public int shipID;
        public string firstName, lastName;

        public HumanShip(Random rnd, int maxID)
        {
            NamesHolder.Init();
            shipID = rnd.Next(0, maxID);
            firstName = NamesHolder.GetFirstName(rnd);
            lastName = NamesHolder.GetLastName(rnd);
        }
    }

    public int GetSpeed()
    {
        var seed = (int) (PlayerDataManager.CurrentSolarSystem.position.x + PlayerDataManager.CurrentSolarSystem.position.y + PlayerDataManager.CurrentSolarSystem.position.z);
        return seed;
    }

    public int GetShipsCount()
    {
        return new Random(GetSpeed()).Next(10, 30);
    }

    [SerializeField] private BotVisual botPrefab;
    [SerializeField] private List<LocationHolder> locations = new List<LocationHolder>();
    [SerializeField] private List<HumanShip> ships = new List<HumanShip>();

    private void Start()
    {
        for (int i = 0; i < PlayerDataManager.CurrentSolarSystem.stations.Count; i++)
        {
            locations.Add(new LocationHolder(PlayerDataManager.CurrentSolarSystem.stations[i].name));
        }

        for (int i = 0; i < PlayerDataManager.CurrentSolarSystem.belts.Count; i++)
        {
            locations.Add(new LocationHolder(PlayerDataManager.CurrentSolarSystem.belts[i].name));
        }

        var rnd = new Random(GetSpeed());
        var count = GetShipsCount();
        if (locations.Count < 2)
        {
            count = Mathf.Clamp(count, 2, 10);
        }

        var date = DateTime.Now;
        var positionsRnd = new Random(GetSpeed() + date.Year + date.Month + date.Day + date.Hour);
        for (int i = 0; i < count; i++)
        {
            var locID = positionsRnd.Next(-1, locations.Count);
            var ship = new HumanShip(rnd, botPrefab.ships.Count);
            if (locID >= 0)
            {
                if (locations[locID].humans.Count < 3)
                {
                    locations[locID].humans.Add(ship);
                    continue;
                }
            }
            
            ships.Add(ship);
        }
    }
}
