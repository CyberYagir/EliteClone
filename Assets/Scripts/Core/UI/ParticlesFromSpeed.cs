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
            particleSystem.startSpeed = Player.Player.inst.control.speed * 10;
            var rm = particleSystem.emission;
            rm.rateOverTime = Player.Player.inst.control.speed * 10;

            var f = particleSystem.forceOverLifetime;
            f.x = Mathf.Sign(Player.Player.inst.control.yaw) * -20 * (Player.Player.inst.control.yaw == 0 ? 0 : 1);
            f.y = Player.Player.inst.control.vertical * -20;
        }
    }
}
