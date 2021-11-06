using System.Collections.Generic;

[System.Serializable]
public class Planet: SpaceObject
{
    public List<SpaceObject> sattelites = new List<SpaceObject>();

    public enum GroundType { Gas, Stone};
    public enum AtmosphereType { None, NotDense, Dense, ExtraDense };
    public enum FluidsType { Yes, No};
    public enum OceansType { No, Small, Large, All};

    public GroundType ground;
    public AtmosphereType atmosphere;
    public FluidsType fluids;
    public OceansType oceans;

    public SpaceObject parent;

    public float temperature;

    

    public Planet()
    {

    }
}
