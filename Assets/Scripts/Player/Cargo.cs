using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;

public class Cargo : MonoBehaviour
{
    public class ItemData
    {
        public string idName;
        public float value;
    }
    
    private ItemShip currentShip;
    public List<Item> items { get; private set; } = new List<Item>();
    public float tons { get; private set; } = 0;
    public Event OnChangeInventory = new Event();
    
    
    
    
    private void Awake()
    {
        var ship = GetComponent<Ship>();
        if (ship != null)
        {
            currentShip = ship.GetShip();
        }
    }


    public void CustomInit(PlayerData data, ItemShip ship)
    {
        currentShip = ship;
        LoadData(data.items);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            var item = ItemsManager.GetCredits();
            item.amount.SetValue(1000);
            AddItem(item);
        }
    }

    public int GetCredits()
    {
        var credit = FindItem("credit");
        if (credit != null)
        {
            return (int)credit.amount.Value;
        }
        else
        {
            return 0;
        }
    }

    public void AddCredits(float count)
    {
        var credits = ItemsManager.GetCredits().Clone();
        credits.amount.SetValue(count);
        AddItem(credits, true);
        OnChangeInventory.Run();
    }
    public bool RemoveCredits(float remove, bool updateInventory = false)
    {
        var credit = FindItem("credit");
        if (credit != null)
        {
            if (credit.amount.Value >= remove)
            {
                credit.amount.SubValue(remove);
                if (updateInventory)
                {
                    OnChangeInventory.Run();
                }
                return true;
            }
        }
        return false;
    }

    public void UpdateInventory()
    {
        OnChangeInventory.Run();
    }
    
    
    public List<ItemData> GetData()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        for (int i = 0; i < items.Count; i++)
        {
            itemDatas.Add(new ItemData() {idName = items[i].id.idname, value = items[i].amount.Value});
        }
        return itemDatas;
    }

    public void LoadData(List<ItemData> itemDatas)
    {
        foreach (var data in itemDatas)
        {
            var item = ItemsManager.GetItem(data);
            AddItem(item, false);
        }

        OnChangeInventory.Run();
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

        if (tons + itemsWeight < currentShip.data.maxCargoWeight)
        {
            if (add)
            {
                foreach (var item in its)
                {
                    AddItem(item, false);
                }
            }
            OnChangeInventory.Run();
            return true;
        }
        OnChangeInventory.Run();
        return false;
    }
    public bool AddItem(Item item, bool callEvent = true)
    {
        var itemMass = item.amount.Value * (float) item.GetKeyPair(KeyPairValue.Mass);
        
        var findedItem = items.Find(x => x.id.idname == item.id.idname);
        if (findedItem)
        {
            var mass = (float) findedItem.GetKeyPair(KeyPairValue.Mass);
            var tonsWithOut = tons - (findedItem.amount.Value * mass);
            var canAddByWeight = tonsWithOut + (item.amount.Value * mass) < currentShip.data.maxCargoWeight;
            if (findedItem.amount.Value + item.amount.Value < findedItem.amount.Max && canAddByWeight)
            {
                findedItem.amount.AddValue(item.amount.Value);
                tons += (item.amount.Value * mass);
                if (callEvent) OnChangeInventory.Run();
                return true;
            }else if (canAddByWeight)
            {
                AddToInventory(item);
                if (callEvent) OnChangeInventory.Run();
                return true;
            }
        }
        else
        {
            if (tons + itemMass < currentShip.data.maxCargoWeight)
            {
                AddToInventory(item);
                if (callEvent) OnChangeInventory.Run();
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

    public Item RemoveItem(string idName, float value = 1, bool callEvent = false)
    {
        var item = FindItem(idName);
        var removed = item.Clone();
        removed.amount.SetValue(0);
        if (item)
        {

            var itemMass = (float) item.GetKeyPair(KeyPairValue.Mass);
            for (int i = 0; i < value; i++)
            {
                if (item.amount.Value > 0)
                {
                    item.amount.SubValue(1);
                    tons -= itemMass;
                    removed.amount.AddValue(1);
                }
            }
            if (item.amount.Value == 0)
            {
                items.Remove(item);
                tons -= item.amount.Value * itemMass;
                Destroy(item);
                OnChangeInventory.Run();
            }
        }

        if (removed.amount.Value == 0)
        {
            if (callEvent) OnChangeInventory.Run();
            return null;
        }
        else
        {
            if (callEvent) OnChangeInventory.Run();
            return removed;
        }
    }

    public bool RemoveItems(List<Item> its)
    {
        if (ContainItems(its))
        {
            for (int i = 0; i < its.Count; i++)
            {
                RemoveItem(its[i].id.idname, its[i].amount.Value, false);
            }
            OnChangeInventory.Run();
            return true;
        }
        return false;
    }
}
