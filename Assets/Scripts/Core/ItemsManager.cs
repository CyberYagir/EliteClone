using System.Collections.Generic;
using System.Linq;
using Core.Game;
using Core.Player;
using UnityEngine;
using Random = System.Random;

namespace Core
{
    public class ItemsManager : MonoBehaviour
    {
        private static ItemList itemList;
        private static QuestsRewards itemRewards;
        private static ShipList shipList;

        private void Awake()
        {
            Init();
        }

        public static void Init()
        {
            itemList = Resources.LoadAll<ItemList>("").ToList().First();
            itemRewards = Resources.LoadAll<QuestsRewards>("").ToList().First();
            shipList = Resources.LoadAll<ShipList>("").ToList().First();
        }

        public static bool IsInited()
        {
            return itemList != null;
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
        public static Item GetMineralItem(Random rnd)
        {
            return GetItem(itemRewards.canBeMineral[rnd.Next(0, itemRewards.canBeMineral.Count)]);
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
    
        public static List<Item> GetItemList()
        {
            return itemList.GetItemList();
        }
    
        public static List<ItemShip> GetShisList()
        {
            return shipList.GetShipsList();
        }
    
    
        public static ItemShip GetShipItem(ItemShip id)
        {
            return shipList.Get(id);
        }
        public static ItemShip GetShipItem(int id)
        {
            return shipList.Get(id);
        }
        public static ShipList.ShipListData GetShipCost(ItemShip id)
        {
            return shipList.GetData(id.shipName);
        }
        public static ItemShip GetShipItem(string name)
        {
            return shipList.Get(name);
        }
    }
}
