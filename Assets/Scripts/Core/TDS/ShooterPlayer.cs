using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.TDS
{
    public class ShooterPlayer : Shooter
    {
        public static ShooterPlayer Instance;
        public ShooterInventory inventory { get; private set; }
        public ShooterPlayerController controller { get; private set; }
        public ShooterAnimator animator { get; private set; }
        public ShooterWeaponSelect weaponSelect { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            inventory = GetComponent<ShooterInventory>();
            controller = GetComponent<ShooterPlayerController>();
            animator = GetComponentInChildren<ShooterAnimator>();
            weaponSelect = GetComponentInChildren<ShooterWeaponSelect>();
        }

        public bool HaveWeapon()
        {
            return inventory.items.Count > 0;
        }
        
        

    }
}
