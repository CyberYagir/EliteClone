using System;
using Core.Bot;
using Core.Game;
using UnityEngine;

namespace Core.PlayerScripts.Weapon
{
    public class WeaponMachinegun : Weapon
    {
        private ParticleSystem bullets = null;
        protected override void InitData()
        {
            bullets = Instantiate(options.GetObject("Bullets") as GameObject, cacheHolder.transform).GetComponent<ParticleSystem>();
            bullets.transform.localPosition = Vector3.zero;
            if (GetComponentInParent<BotBuilder>())
            {
                bullets.GetComponent<ParticleCollision>().SetBot();
            }
        }

        private void Update()
        {
            time += Time.deltaTime;
        }

        protected override void Attack()
        {
            base.Attack();
            if (time > cooldown)
            {
                bullets.Emit(1);
            }
        }
    }
}
