using Core.Location;

namespace Core.Systems.InteractivePoints
{
    class WorldInteractivePointStation : WorldInteractivePoint
    {
        public override void InitLocation(PlayerDataManager playerDataManager, LocationGenerator locationGenerator)
        {
            base.InitLocation(playerDataManager, locationGenerator);
            
            locationGenerator.SetSpawnedLocation(gameObject, LocationGenerator.SpawnedLocation.SpawnedLocationType.OrbitalStation);
            
            GetComponent<WorldOrbitalStation>().Init(playerDataManager);
        }
    }
}