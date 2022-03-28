using Random = System.Random;

namespace Core.Galaxy
{
    [System.Serializable]
    public class Belt : SpaceObject
    {    
        public enum ClasterType { Stones, Metals, Crystals, Mixed };

        [System.Serializable]
        public class BeltData
        {
            public ClasterType clasterType;
            public int meteorsCount;
            public BeltData()
            {
            }
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
