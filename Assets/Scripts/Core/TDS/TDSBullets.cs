using System;
using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.TDS
{
    public class TDSBullets : MonoBehaviour
    {
        public float damage;
        private ParticleSystem particleSystem;
        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        public ParticleSystem GetParticles()
        {
            return particleSystem;
        }

        private void OnParticleCollision(GameObject other)
        {
            IDamagable damagable = other.GetComponent<IDamagable>();
            if (damagable == null)
            {
                damagable = other.GetComponentInParent<IDamagable>();
            }
            damagable?.TakeDamage(damage);
        }
    }
}
