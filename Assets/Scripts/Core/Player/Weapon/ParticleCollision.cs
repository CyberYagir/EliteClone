using System;
using System.Collections;
using System.Collections.Generic;
using Core.Game;
using UnityEngine;


namespace Core.PlayerScripts.Weapon
{
    public class ParticleCollision : MonoBehaviour
    {
        private Weapon weapon;
        private float damage = 0;

        private void Awake()
        {
            weapon = GetComponentInParent<Weapon>();
            damage = (float)weapon.Current().GetKeyPair(KeyPairValue.Damage);
        }

        private void OnParticleCollision(GameObject other)
        {
            IDamagable damagable = null;
            damagable = other.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
            else
            {
                other.GetComponentInParent<IDamagable>()?.TakeDamage(damage);
            }
                
        }
    }
}
