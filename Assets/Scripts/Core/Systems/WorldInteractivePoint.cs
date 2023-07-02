using UnityEngine;
using UnityEngine.Events;

namespace Core.Systems
{
    public class WorldInteractivePoint : MonoBehaviour
    {
        public Transform spawnPoint;
        public UnityEvent initEvent;

        public Transform SpawnPoint => spawnPoint;
    }
}
