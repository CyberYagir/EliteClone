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
        private bool isBot;
        private void Awake()
        {
            weapon = GetComponentInParent<Weapon>();
            damage = (float)weapon.Current().GetKeyPair(KeyPairValue.Damage);
        }

        public void SetBot()
        {
            isBot = true;
            var part = GetComponent<ParticleSystem>();
            var collision = part.collision;
            collision.collidesWith = LayerMask.GetMask("ShipMesh");
        }

        private void OnParticleCollision(GameObject other)
        {
            IDamagable damagable = null;
            damagable = other.GetComponent<IDamagable>();
            if (damagable == null)
            {
                damagable = other.GetComponentInParent<IDamagable>();
            }
            if (damagable != null)
            {  
                damagable.TakeDamage(damage);
            }
        }
    }
}
