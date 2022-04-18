using System;
using UnityEngine;

namespace Core.PlayerScripts.Weapon
{
    public class WeaponMachinegun : Weapon
    {
        private ParticleSystem bullets = null;
        private float time;
        protected override void InitData()
        {
            bullets = Instantiate(options.GetObject("Bullets") as GameObject, cacheHolder.transform).GetComponent<ParticleSystem>();
            bullets.transform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            time += Time.deltaTime;
        }

        protected override void Attack()
        {
            base.Attack();
            if (time > options.cooldown)
            {
                bullets.Emit(1);
            }
        }
    }
}
