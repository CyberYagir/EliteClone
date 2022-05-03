using System;
using System.Collections;
using System.Collections.Generic;
using Core.Game;
using UnityEngine;

namespace Core.TDS.Weapons
{
    public class TDSWeapon : MonoBehaviour
    {
        protected Item currentItem;
        protected TDSWeaponOptions options;
        protected float coodown;
        protected float damage;
        protected float energy;
        protected Transform point;
        protected float time;
        protected Shooter actor;
        protected ParticleSystem bullets;

        public virtual void Init(Item item, TDSWeaponOptions options, Transform wpoint)
        {
            currentItem = item;
            actor = GetComponent<Shooter>();
            actor.attack.OnShoot += CheckEnergy;
            point = wpoint;

            coodown = (float) currentItem.GetKeyPair(KeyPairValue.Cooldown);
            damage = (float) currentItem.GetKeyPair(KeyPairValue.Damage);
            energy = (float) currentItem.GetKeyPair(KeyPairValue.Energy);
            bullets = Instantiate(options.bullet, wpoint.transform.position, wpoint.transform.rotation).GetComponent<ParticleSystem>();
            bullets.transform.parent = point;
        }

        public void CheckEnergy()
        {
            if (actor.data.RemoveEnergy(energy))
            {
                Attack();
            }
        }

        public virtual void Attack()
        {

        }

        private void OnDestroy()
        {
            foreach (Transform child in point)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
