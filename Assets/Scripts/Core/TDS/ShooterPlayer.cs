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
        public ShooterPlayerController controller;
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            inventory = GetComponent<ShooterInventory>();
            controller = GetComponent<ShooterPlayerController>();
        }

        public bool HaveWeapon()
        {
            return inventory.items.Count > 0;
        }
    }
}
