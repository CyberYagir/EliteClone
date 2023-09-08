using Core.Galaxy;
using Core.Location;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class WorldDataHandler
    {
        private SolarSystem currentSolarSystem;
        
        [SerializeField] private Player shipPlayer;
        [SerializeField] private LocationGenerator currentLocationGenerator;
        [SerializeField] private SolarSystemGenerator currentSolarGenerator;
        [SerializeField] private SolarSystemShips currentShipsData;
        
        [Space(50)]
        [SerializeField] private int galaxySeed = -1;
        [SerializeField] private Player playerPrefab;

        
        public SolarSystem CurrentSolarSystem => currentSolarSystem;
        public int GalaxySeed => galaxySeed;
        public Player ShipPlayer => shipPlayer;

        public LocationGenerator CurrentLocationGenerator => currentLocationGenerator;
        public SolarSystemGenerator CurrentSolarGenerator => currentSolarGenerator;

        public SolarSystemShips CurrentShipsData => currentShipsData;

        public Event OnChangeLocation = new Event();

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

        public Player TryCreatePlayerShip()
        {
            if (shipPlayer == null)
            {
                return Object.Instantiate(playerPrefab);
            }
            else
            {
                return shipPlayer;
            }
        }


        public void SetLocation(LocationGenerator generator)
        {
            currentLocationGenerator = generator;
            OnChangeLocation.Run();
        }
        public void SetLocation(SolarSystemGenerator generator)
        {
            currentSolarGenerator = generator;
            OnChangeLocation.Run();
        }

        public void SetSystemShips(SolarSystemShips solarSystemShips)
        {
            currentShipsData = solarSystemShips;
        }
    }
}