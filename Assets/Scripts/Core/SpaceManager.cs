using Core.PlayerScripts;
using UnityEngine;

namespace Core
{
    public class SpaceManager : StartupObject
    {
        public override void Init(PlayerDataManager playerDataManager)
        {
            base.Init(playerDataManager);
            Player.ChangeScene(); //Есть на каждой локе чтобы триггерить эвент смены локации
        }
    }
}
