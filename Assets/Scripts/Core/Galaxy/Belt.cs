using System;

namespace Core.Galaxy
{
    [Serializable]
    public class Belt : SpaceObject
    {    
        public enum ClasterType { Stones, Metals, Crystals, Mixed }

        [Serializable]
        public class BeltData
        {
            public ClasterType clasterType;
            public int meteorsCount;
        }

        public BeltData beltData;
    
    
    
    
        public BeltData GenBeltData(Random rnd)
        {
            var belt = new BeltData();
            belt.clasterType = (ClasterType)rnd.Next(0, 3);
            belt.meteorsCount = rnd.Next(200, 3000);
            return belt;
        }
    
        public Belt()
        {
        
        }

        public Belt(Random rnd)
        {
            beltData = GenBeltData(rnd);
        }
    }
}
