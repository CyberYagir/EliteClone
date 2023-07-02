using Core.Location;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Systems.InteractivePoints
{
    public class WorldInteractivePoint : MonoBehaviour
    {
        public Transform spawnPoint;

        public Transform SpawnPoint => spawnPoint;

        public virtual void InitLocation(PlayerDataManager playerDataManager, LocationGenerator locationGenerator)
        {
            
        }
    }
}
