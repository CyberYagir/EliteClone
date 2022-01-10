using System;

[System.Serializable]

public class Belt : SpaceObject
{
    public enum ClasterType { Stones, Metals, Crystals, Mixed };

    public ClasterType claster;
    
    public int meteorsCount;


    public Belt(Random rnd)
    {
        claster = (ClasterType)rnd.Next(0, 3);
        meteorsCount = rnd.Next(200, 3000);
    }

    public Belt()
    {
        
    }
}
