using Newtonsoft.Json;
using System.Collections.Generic;

[System.Serializable]
public class SolarSystem:SpaceObject
{
    public List<Star> stars = new List<Star>();
    [JsonIgnore]
    public List<Planet> planets = new List<Planet>();
}
