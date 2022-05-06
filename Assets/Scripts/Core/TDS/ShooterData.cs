using System;
using Core;
using Core.PlayerScripts;
using UnityEngine;
using Event = Core.Event;

namespace Core.TDS
{
    public class ShooterData : MonoBehaviour, IDamagable
    {
        [SerializeField] private float heath = 100;
        [SerializeField] private float energy = 1000;
        [SerializeField] private bool god;
        private float maxHeath = 100;
        private float maxEnergy = 1000;

        public Event UpdateData = new Event();


        private Damager damager;

        private void Start()
        {
            damager = GetComponent<Damager>();
            
        }

        public void TakeDamage(float damage)
        {
            damager.TakeDamage(ref heath, damage);
            UpdateData.Run();

            if (god)
            {
                if (heath <= 0)
                {
                    heath = 1;
                }
            }
            if (heath <= 0)
            {
                var dir = (ShooterPlayer.Instance.transform.position-transform.position);
                
                GetComponent<Shooter>().Death(transform.position + dir + Vector3.up);
            }
        }


        public bool RemoveEnergy(float val)
        {
            if (energy > val)
            {
                energy -= val;
                UpdateData.Run();
                return true;
            }

            return false;
        }

        public float GetHealth()
        {
            return heath / maxHeath;
        }

        public float GetEnergy()
        {
            return energy / maxEnergy;
        }

    }

}
