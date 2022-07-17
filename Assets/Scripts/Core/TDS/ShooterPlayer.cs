using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        public void Disable(Transform target, bool state, bool look)
        {
            controller.enabled = state;
            controller.GetRigidbody().isKinematic = !state;
            if (target != null)
            {
                controller.SetPointPose(target.position);
            }

            if (look)
            {
                transform.DOLookAt(new Vector3(target.position.x, transform.position.y, target.position.z), 0.5f);
            }
        }
        

    }
}
