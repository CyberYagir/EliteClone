using System.Collections.Generic;

namespace Core.Galaxy
{
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
            mass = GalaxyGenerator.NextDecimal(rnd, 0.05f, 0.25f);
            if (!isStattelite)
            {
                type = (PlanetType) rnd.Next(0, 2);
            }
            radius = GalaxyGenerator.NextDecimal(rnd, 0.1f, 0.2f);
            if (type == PlanetType.Gas)
            {
                radius *= 2 * ((1 + (float)rnd.NextDouble()) + 0.5f);
                if (radius <= 0.2f)
                {
                    radius = 0.3f;
                }
            }
        }
    }
}
