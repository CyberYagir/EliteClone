using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected int weaponID;
    protected Item currentItem;
    protected GameObject cacheHolder;
    protected WeaponOptionsItem options;
    protected Camera camera;
    
    public void Init(int shootKey, Item current, WeaponOptionsItem opt)
    {
        currentItem = current;
        weaponID = shootKey;
        options = opt;
        
        camera = Camera.main;

        cacheHolder = SpawnCacheHolder();
        
        if (Player.inst != null)
        {
            Player.inst.attack.OnShoot += CheckIsCurrentWeapon;
            Player.inst.attack.OnHold += OnHold;
            Player.inst.attack.OnHold += OnHoldDown;
        }

        InitData();
    }

    protected virtual void InitData(){}
    
    
    private GameObject SpawnCacheHolder()
    {
        var holder = new GameObject("SlotData");
        holder.transform.parent = transform;
        holder.transform.localPosition = Vector3.zero;
        holder.transform.localEulerAngles = Vector3.zero;
        holder.layer = LayerMask.NameToLayer("Main");
        return holder;
    }
    
    private void CheckIsCurrentWeapon(int shootKey)
    {
        if (shootKey == weaponID)
        {
            Attack();
        }
    }

    private void OnHold(int shootKey)
    {
        if (shootKey == weaponID)
        {
            NotAttack();
        }
    } 
    private void OnHoldDown(int shootKey)
    {
        if (shootKey == weaponID)
        {
            AttackDown();
        }
    }
    

    protected virtual void NotAttack()
    {
        
    }
    protected virtual void AttackDown()
    {
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
        Destroy(cacheHolder.gameObject);
        ClearData();
    }
}
