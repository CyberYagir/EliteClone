using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    private Ship currentShip;
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private float tons = 0;

    private void Awake()
    {
        currentShip = GetComponent<Ship>();
    }

    public Item FindItem(string itemName)
    {
        return items.Find(x => x.id.idname == itemName);
    }
    public bool ContainItem(string itemName)
    {
        return FindItem(itemName) != null;
    }
    public bool ContainItem(string itemName, float value)
    {
        var item = FindItem(itemName);
        if (item)
        {
            if (item.amount.Value >= value)
            {
                return true;
            }   
        }

        return false;
    }

    public bool ContainItems(List<Item> its)
    {
        foreach (var item in its)
        {
            if (!ContainItem(item.id.idname, item.amount.Value))
            {
                return false;
            }
        }
        return true;
    }
    
    public bool AddItems(List<Item> its, bool add = true)
    {
        float itemsWeight = 0;
        foreach (var item in its)
        {
            itemsWeight += item.amount.Value * (float)item.GetKeyPair(KeyPairValue.Mass);
        }

        if (tons + itemsWeight < currentShip.GetShip().data.maxCargoWeight)
        {
            if (add)
            {
                foreach (var item in its)
                {
                    AddItem(item);
                }
            }
            return true;
        }

        return false;
    }
    public bool AddItem(Item item)
    {
        var itemMass = item.amount.Value * (float) item.GetKeyPair(KeyPairValue.Mass);
        
        var findedItem = items.Find(x => x.id.idname == item.id.idname);
        if (findedItem)
        {
            var mass = (float) findedItem.GetKeyPair(KeyPairValue.Mass);
            var tonsWithOut = tons - (findedItem.amount.Value * mass);
            var canAddByWeight = tonsWithOut + (item.amount.Value * mass) < currentShip.GetShip().data.maxCargoWeight;
            if (findedItem.amount.Value + item.amount.Value < findedItem.amount.Max && canAddByWeight)
            {
                findedItem.amount.AddValue(item.amount.Value);
                tons += (item.amount.Value * mass);
                return true;
            }else if (canAddByWeight)
            {
                AddToInventory(item);
                return true;
            }
        }
        else
        {
            if (tons + itemMass < currentShip.GetShip().data.maxCargoWeight)
            {
                AddToInventory(item);
                return true;
            }
        }
        return false;
    }
    public void AddToInventory(Item item)
    {
        items.Add(item.Clone());
        tons +=  item.amount.Value * (float) item.GetKeyPair(KeyPairValue.Mass);
    }

    public Item RemoveItem(string itemName, float value = 1)
    {
        var item = FindItem(itemName);
        var removed = item.Clone();
        removed.amount.SetValue(0);
        if (item)
        {
            for (int i = 0; i < value; i++)
            {
                if (item.amount.Value > 0)
                {
                    item.amount.SubValue(1);
                    removed.amount.AddValue(1);
                }
            }

            if (item.amount.Value == 0)
            {
                items.Remove(item);
                Destroy(item);
            }
        }

        if (removed.amount.Value == 0)
        {
            return null;
        }
        else
        {
            return removed;
        }
    }

    public bool RemoveItems(List<Item> its)
    {
        if (ContainItems(its))
        {
            for (int i = 0; i < its.Count; i++)
            {
                RemoveItem(its[i].id.idname, its[i].amount.Value);
            }

            return true;
        }
        return false;
    }
}
