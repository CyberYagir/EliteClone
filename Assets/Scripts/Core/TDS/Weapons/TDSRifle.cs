using System.Collections;
using System.Collections.Generic;
using Core.TDS.Weapons;
using UnityEngine;

namespace Core.TDS.Weapons
{
    public class TDSRifle : TDSWeapon
    {
        public override void Attack()
        {
            base.Attack();
            print("Attack");
        }
    }
}
