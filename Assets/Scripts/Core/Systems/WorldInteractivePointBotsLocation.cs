using Core.Location;

namespace Core.Systems.InteractivePoints
{
    class WorldInteractivePointBotsLocation : WorldInteractivePoint
    {
        public override void InitLocation(PlayerDataManager playerDataManager, LocationGenerator locationGenerator)
        {
            base.InitLocation(playerDataManager, locationGenerator);
            
            GetComponent<LocationBotPoint>().Init(
                locationGenerator.GetComponent<SolarSystemShips>(),
                playerDataManager.WorldHandler);
        }
    }
}