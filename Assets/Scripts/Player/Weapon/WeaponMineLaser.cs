using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMineLaser : WeaponLaser
{
    protected override void InitData()
    {
        base.InitData();
        OnSpawnDecal += DamageAsteroid;
    }

    public void DamageAsteroid(RaycastHit obj)
    {
        var meteor = obj.transform.GetComponent<Meteor>();
        if (meteor)
        {
            meteor.TakeDamage(options.damage);
            if (meteor.GetHealth() > 0)
            {
                meteor.SpawnDrop(obj.normal, obj.point);
            }
        }
    }
}
