using Core.Galaxy;
using Core.PlayerScripts;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class WorldDataHandler
    {
        private SolarSystem currentSolarSystem;
        [SerializeField] private int galaxySeed = -1;
        [SerializeField] private Player shipPlayer;

        public SolarSystem CurrentSolarSystem => currentSolarSystem;
        public int GalaxySeed => galaxySeed;
        public Player ShipPlayer => shipPlayer;


        public void ChangeSolarSystem(SolarSystem solarSystem) => currentSolarSystem = solarSystem;
        public void ChangeGalaxySeed(int newSeed) => galaxySeed = newSeed;
        public void ClarSolarSystem() => ChangeSolarSystem(null);
        public void SetShipPlayer(Player ship) => shipPlayer = ship;

        public void DestroyShipPlayer()
        {
            if (shipPlayer != null)
            {
                Object.Destroy(shipPlayer.gameObject);
            }
        }
    }
}