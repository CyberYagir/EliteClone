using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected int weaponID;
    protected Item currentItem;
    public void Init(int shootKey, Item current)
    {
        currentItem = current;
        weaponID = shootKey;
        if (Player.inst != null)
        {
            Player.inst.attack.OnShoot += CheckIsCurrentWeapon;
        }
    }

    private void CheckIsCurrentWeapon(int shootKey)
    {
        if (shootKey == weaponID)
        {
            Attack();
        }
    }
    
    protected virtual void Attack()
    {
    }

    protected virtual void Reload()
    {
        
    }

    protected virtual void ClearData()
    {
        
    }
    
    private void OnDestroy()
    {
        ClearData();
    }
}
