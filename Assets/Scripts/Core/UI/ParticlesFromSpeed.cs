using Core.PlayerScripts;
using UnityEngine;

namespace Core.UI
{
    public class ParticlesFromSpeed : MonoBehaviour
    {
        private new ParticleSystem particleSystem;
        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
        void Update()
        {
            particleSystem.startSpeed = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Control.speed * 10;
            var rm = particleSystem.emission;
            rm.rateOverTime = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Control.speed * 10;

            var f = particleSystem.forceOverLifetime;
            f.x = Mathf.Sign(PlayerDataManager.Instance.WorldHandler.ShipPlayer.Control.yaw) * -20 * (PlayerDataManager.Instance.WorldHandler.ShipPlayer.Control.yaw == 0 ? 0 : 1);
            f.y = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Control.vertical * -20;
        }
    }
}
