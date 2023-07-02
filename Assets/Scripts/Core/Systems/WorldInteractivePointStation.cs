﻿using Core.Location;

namespace Core.Systems.InteractivePoints
{
    class WorldInteractivePointStation : WorldInteractivePoint
    {
        public override void InitLocation(PlayerDataManager playerDataManager, LocationGenerator locationGenerator)
        {
            base.InitLocation(playerDataManager, locationGenerator);
            GetComponent<WorldOrbitalStation>().Init();
        }
    }
}