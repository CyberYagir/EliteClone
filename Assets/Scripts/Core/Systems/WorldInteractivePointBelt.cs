using Core.Location;

namespace Core.Systems.InteractivePoints
{
    class WorldInteractivePointBelt : WorldInteractivePoint
    {
        public override void InitLocation(PlayerDataManager playerDataManager, LocationGenerator locationGenerator)
        {
            base.InitLocation(playerDataManager, locationGenerator);
            GetComponent<BeltGenerator>().Init();
        }
    }
}