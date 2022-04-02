﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Core.Galaxy
{
    [System.Serializable]
    public class NeighbourSolarSytem
    {
        public string solarName;
        public DVector position;
    }

    [System.Serializable]
    public class SolarSystem:SpaceObject
    {
        public List<Star> stars = new List<Star>();
        public List<Planet> planets = new List<Planet>();
        public List<Belt> belts = new List<Belt>();
        public List<OrbitStation> stations = new List<OrbitStation>();
        public List<NeighbourSolarSytem> sibligs = new List<NeighbourSolarSytem>();

        public void SetName()
        {
            name = stars[0].name;
        }
    }
}