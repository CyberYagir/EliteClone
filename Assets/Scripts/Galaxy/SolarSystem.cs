using Newtonsoft.Json;
using System.Collections.Generic;

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
    public List<NeighbourSolarSytem> sibligs = new List<NeighbourSolarSytem>();
}
