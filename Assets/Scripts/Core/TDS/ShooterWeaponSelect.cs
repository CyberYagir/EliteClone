using System;
using System.Collections;
using System.Collections.Generic;
using Core.Game;
using Core.TDS;
using Core.TDS.Weapons;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Core.TDS
{
    public class ShooterWeaponSelect : MonoBehaviour
    {
        [SerializeField] private ShooterWeaponList weaponsData;
        [SerializeField] private int currentWeapon;
        [SerializeField] private ShooterInventory weapons;
        [SerializeField] private Transform leftH, rightH;

        private void Update()
        {
            for (int i = 0; i < weapons.items.Count; i++)
            {
                if (Input.GetKeyDown((i+1).ToString()))
                {
                    if (currentWeapon == i)
                    {
                        RemoveWeapon();
                        currentWeapon = -1;
                    }
                    else
                    {
                        RemoveWeapon();
                        currentWeapon = i;
                        ActiveWeapon();
                    }
                }
            }
        }

        public void ActiveWeapon()
        {
            var data = weaponsData.weapons[(int) (float) weapons.items[currentWeapon].GetKeyPair(KeyPairValue.Value)];
            var weapon = (TDSWeapon)gameObject.AddComponent(Type.GetType(data.weaponScript + data.script.name));
            data.mesh.SetActive(true);
            data.options.leftH.Set(leftH);
            data.options.rightH.Set(rightH);
            weapon.Init(weapons.items[currentWeapon].Clone(), data.options, data.bulletPoint);
        }
        public void RemoveWeapon(){
            ShooterPlayer.Instance.animator.SetLayerValue(1, 0);
            if (currentWeapon != -1)
            {
                var data = weaponsData.weapons[(int) (float) weapons.items[currentWeapon].GetKeyPair(KeyPairValue.Value)];
                data.mesh.SetActive(false);
            }

            leftH.GetComponentInParent<ChainIKConstraint>().weight = 0;
            rightH.GetComponentInParent<ChainIKConstraint>().weight = 0;
            Destroy(GetComponent<TDSWeapon>());
        }
    }
}
