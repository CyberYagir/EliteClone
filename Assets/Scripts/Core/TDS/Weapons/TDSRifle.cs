using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS.Weapons;
using UnityEngine;

namespace Core.TDS.Weapons
{
    public class TDSRifle : TDSWeapon
    {
        private void Update()
        {
            time += Time.deltaTime;
        }

        public override void Attack()
        {
            if (time > coodown)
            {
                bullets.Emit(1);
                time = 0;
            }
        }
    }
}
