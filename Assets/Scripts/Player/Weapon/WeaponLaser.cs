using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLaser : Weapon
{
    protected override void Attack()
    {
        base.Attack();
        print(weaponID);
    }
}
