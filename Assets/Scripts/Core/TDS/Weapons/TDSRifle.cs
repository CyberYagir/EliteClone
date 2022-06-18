using System;
using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
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
                CameraShake.Shake();
                bullets.Emit(1);
                time = 0;
            }
        }
    }
}
