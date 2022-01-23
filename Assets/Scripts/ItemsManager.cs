using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using Quests;
using UnityEngine;
using Random = System.Random;

public class ItemsManager : MonoBehaviour
{
    private static ItemList itemList;
    private static QuestsRewards itemRewards;
    private static ShipList shipList;

    private void Awake()
    {
        itemList = Resources.LoadAll<ItemList>("").ToList().First();
        itemRewards = Resources.LoadAll<QuestsRewards>("").ToList().First();
        shipList = Resources.LoadAll<ShipList>("").ToList().First();
    }

    public static Item GetCredits()
    {
        return itemRewards.creditItem;
    }

    public static Item GetRewardItem(Random rnd)
    {
        return GetItem(itemRewards.canBeRewarded[rnd.Next(0, itemRewards.canBeRewarded.Count)]);
    }

    public static Item GetTransferedItem(Random rnd)
    {
        return GetItem(itemRewards.canBeTransfered[rnd.Next(0, itemRewards.canBeTransfered.Count)]);
    }

    public static Item GetItem(int id)
    {
        return itemList.Get(id);
    }

    public static Item GetItem(string id)
    {
        return itemList.Get(id);
    }
    
    public static Item GetItem(Cargo.ItemData data)
    {
        var item = itemList.Get(data.idName);
        item.amount.SetValue(data.value);
        return item;
    }

    public static Item GetItem(Item id)
    {
        return itemList.Get(id);
    }
    
    public static ItemShip GetShipItem(ItemShip id)
    {
        return shipList.Get(id);
    }
    public static ItemShip GetShipItem(string name)
    {
        return shipList.Get(name);
    }
}
