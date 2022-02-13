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

    private float decalTime;
    private LayerMask decalLayer;
    public void Init(int shootKey, Item current, WeaponOptionsItem opt)
    {
        currentItem = current;
        weaponID = shootKey;
        options = opt;
        decalLayer = LayerMask.GetMask("Decals");
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
        decalTime += Time.deltaTime;
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

    protected void SpawnDecal(GameObject decal, Vector3 start, Vector3 dir, RaycastHit initHit)
    {
        if (decalTime >= 1/5f)
        {
            bool addToDecal = false;
            if (Physics.Raycast(start, dir, out RaycastHit hit, options.maxDistance, decalLayer))
            {
                var findedDecal = hit.collider.GetComponent<Decal>();
                if (findedDecal)
                {
                    findedDecal.AddToOpacity();
                    addToDecal = true;
                }
            }

            if (!addToDecal)
            {
                var d = Instantiate(decal, initHit.point, Quaternion.identity);
                d.transform.rotation = Quaternion.FromToRotation(Vector3.forward, initHit.normal);
                d.transform.localRotation *= Quaternion.Euler(90, 0, 0);
                d.transform.parent = initHit.transform;
            }

            decalTime = 0;
        }
    }
    
    private void OnDestroy()
    {
        Destroy(cacheHolder.gameObject);
        ClearData();
    }
}
