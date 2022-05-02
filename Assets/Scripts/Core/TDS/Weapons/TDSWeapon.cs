using System.Collections;
using System.Collections.Generic;
using Core.Game;
using UnityEngine;

namespace Core.TDS.Weapons
{
    public class TDSWeapon : MonoBehaviour
    {
        protected Item currentItem;
        protected float coodown;
        protected float damage;
        
        public virtual void Init(Item item)
        {
            currentItem = item;
            ShooterPlayer.Instance.attack.OnShoot += Attack;

            coodown = (float)currentItem.GetKeyPair(KeyPairValue.Cooldown);
            damage = (float)currentItem.GetKeyPair(KeyPairValue.Damage);
        }

        public virtual void Attack()
        {
            
        }
    }
}
