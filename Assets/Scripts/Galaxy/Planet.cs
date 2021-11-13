using System.Collections.Generic;

[System.Serializable]
public class Planet: SpaceObject
{
    public List<SpaceObject> sattelites = new List<SpaceObject>();

    public enum GroundType { Gas, Stone};
    public enum AtmosphereType { None, NotDense, Dense, ExtraDense };
    public enum FluidsType { No, Yes };
    public enum OceansType { No, Small, Large, All};

    public GroundType ground;
    public AtmosphereType atmosphere;
    public FluidsType fluids;
    public OceansType oceans;

    public float temperature;

    

    public Planet()
    {

    }

    public Planet(System.Random rnd, SpaceObject star, DVector postion)
    {
        this.ground = (GroundType)rnd.Next(0, 2);
        this.atmosphere = (AtmosphereType)rnd.Next(0, 4);
        this.position = postion;
        mass = GalaxyGenerator.NextDecimal(rnd, 0.05m, 0.25m);
        radius = GalaxyGenerator.NextDecimal(rnd, 0.05m, 0.5m);
        if (postion.Dist(star.position) < 10 && atmosphere != AtmosphereType.None)
        {
            this.fluids = (FluidsType)rnd.Next(0, 2);
            if (fluids == FluidsType.Yes)
            {
                this.oceans = (OceansType)rnd.Next(0, 4);
            }
        }
    }
}
