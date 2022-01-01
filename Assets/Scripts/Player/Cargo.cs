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
}
