using System.Collections.Generic;

[System.Serializable]
public class Planet: SpaceObject
{
    public List<Planet> sattelites = new List<Planet>();
    public List<OrbitStation> stations = new List<OrbitStation>();
    public int textureID;

    public PlanetType type;
    public enum PlanetType
    {
        Rock, Gas
    }

    public Planet()
    {

    }

    public Planet(System.Random rnd, DVector postion, bool isStattelite = false)
    {
        this.position = postion;
        mass = GalaxyGenerator.NextDecimal(rnd, 0.05m, 0.25m);
        if (!isStattelite)
        {
            type = (PlanetType) rnd.Next(0, 2);
        }
        radius = GalaxyGenerator.NextDecimal(rnd, 0.1m, 0.2m);
        if (type == PlanetType.Gas)
        {
            radius *= 2 * (decimal)((1 + rnd.NextDouble()) + 0.5f);
            if (radius <= 0.2m)
            {
                radius = 0.3m;
            }
        }
    }
}
