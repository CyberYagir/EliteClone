using UnityEngine;

namespace Core.UI
{
    public abstract class MonoUI: MonoBehaviour
    {
        private PlayerDataManager playerDataManager;
        private WorldDataHandler worldDataHandler;

        public WorldDataHandler WorldDataHandler => worldDataHandler;
        public PlayerDataManager PlayerDataManager => playerDataManager;

        public virtual void OnStoreData(PlayerDataManager playerDataManager)
        {
            this.playerDataManager = playerDataManager;
            this.worldDataHandler = playerDataManager.WorldHandler;
        }
        public virtual void Init()
        {
            
        }
        public virtual void OnUpdate()
        {
            
        }
    }
}