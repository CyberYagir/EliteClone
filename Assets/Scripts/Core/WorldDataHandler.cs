using Core.Galaxy;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class WorldDataHandler
    {
        private SolarSystem currentSolarSystem;
        [SerializeField] private int galaxySeed = -1;

        public SolarSystem CurrentSolarSystem => currentSolarSystem;
        public int GalaxySeed => galaxySeed;


        public void ChangeSolarSystem(SolarSystem solarSystem)
        {
            currentSolarSystem = solarSystem;
        }

        public void ChangeGalaxySeed(int newSeed)
        {
            galaxySeed = newSeed;
        }

        public void ClarSolarSystem()
        {
            ChangeSolarSystem(null);
        }
    }
}