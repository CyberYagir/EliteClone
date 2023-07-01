using Core.Galaxy;
using UnityEngine;

namespace Core.Core.Inject.GlobalDataService
{
    public class SolarSystemService : MonoBehaviour
    { 
        private SolarSystem сurrentSolarSystem;
        private int galaxySeed = -1;

        public int GalaxySeed => galaxySeed;

        public SolarSystem CurrentSolarSystem => сurrentSolarSystem;


        public void SetSolarSystem(SolarSystem сurrentSolarSystem)
        {
            this.сurrentSolarSystem = сurrentSolarSystem;
        }


        public void SetSeed(int galaxySeed)
        {
            this.galaxySeed = galaxySeed;
        }
    }
}