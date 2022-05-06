using System;
using System.Collections;
using System.Collections.Generic;
using Core.Game;
using Core.TDS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.TDS.Bot
{
    public class ShooterBotData : MonoBehaviour
    {
        public Item currentWeapon;
        private Shooter shooter;
        
        private static readonly int CanRandomIdle = Animator.StringToHash("canRandomIdle");
        private static readonly int RandomIdle = Animator.StringToHash("randomIdle");

        private void Start()
        {
            shooter = GetComponent<Shooter>();
            Animator anim = shooter.animator.Get();
            anim.SetBool(CanRandomIdle, true);
            anim.SetInteger(RandomIdle, Random.Range(0, 4));
        }

        private void OnTriggerStay(Collider other)
        {
            if (shooter.isDead && currentWeapon != null)
            {
                if (other.TryGetComponent(out ShooterInventory inventory))
                {
                    var isEmptyInventory = inventory.items.Count == 0;
                    inventory.Add(currentWeapon);
                    if (isEmptyInventory)
                    {
                        inventory.GetComponent<ShooterWeaponSelect>().ChangeWeapon(0);
                    }
                    
                    currentWeapon = null;
                }
            }
        }
    }
}
