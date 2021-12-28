using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Quests;
using UnityEngine;
using Random = System.Random;

public class ItemsManager : MonoBehaviour
{
    private static ItemList itemList;
    private static QuestsRewards itemRewards;

    private void Awake()
    {
        itemList = Resources.LoadAll<ItemList>("").ToList().First();
        itemRewards = Resources.LoadAll<QuestsRewards>("").ToList().First();
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
        return GetItem(itemRewards.canBeTransfered[rnd.Next(0, itemRewards.canBeRewarded.Count)]);
    }

    public static Item GetItem(int id)
    {
        return itemList.Get(id);
    }

    public static Item GetItem(string id)
    {
        return itemList.Get(id);
    }

    public static Item GetItem(Item id)
    {
        return itemList.Get(id);
    }
}