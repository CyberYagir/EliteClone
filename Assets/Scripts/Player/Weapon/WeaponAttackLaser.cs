using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttackLaser : WeaponLaser
{
    protected override void InitData()
    {
        base.InitData();
        OnSpawnDecal += DamageShip;
    }

    public void DamageShip(RaycastHit obj)
    {
        var damagable = obj.transform.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(options.damage);
        }
    }
}
