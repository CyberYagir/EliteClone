using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Game
{
    [CreateAssetMenu(fileName = "", menuName = "Game/ShipList", order = 1)]
    public class ShipList : ScriptableObject
    {

        private static ShipList Instance;
        
        public static ShipList GetData()
        {
            if (Instance == null)
            {
                Instance = Resources.LoadAll<ShipList>("")[0];
            }
            return Instance;
        }
        
        [Serializable]
        public class ShipListData
        {
            [SerializeField] private ItemShip ship;
            [SerializeField] private ValueLimit cost;

            public ShipListData([NotNull] ItemShip ship, [NotNull] ValueLimit cost)
            {
                this.ship = ship;
                this.cost = cost;
            }

            public ValueLimit Cost => cost;
            public ref ItemShip Ship => ref ship;
            public string ShipName => ship.shipName;
        }
        [SerializeField] private List<ShipListData> items = new List<ShipListData>();

        public void SetList(List<ItemShip> newList)
        {
            for (int i = 0; i < newList.Count; i++)
            {
                var find = items.Find(x => x.ShipName == newList[i].shipName);
                if (find == null)
                {
                    items.Add(new ShipListData(newList[i], new ValueLimit()));
                }
            }
        }

        public ItemShip Get(string idName) => items.Find(x => x.ShipName == idName).Ship.Clone();
        public ItemShip Get(int id) => items[id].Ship.Clone();
        public ItemShip Get(ItemShip original) => items.Find(x => x.Ship == original).Ship.Clone();

        public ShipListData GetData(string idName) => items.Find(x => x.ShipName == idName);
        public List<ItemShip> GetShipsList()
        {
            var list = new List<ItemShip>();
            for (int i = 0; i < items.Count; i++)
            {
                list.Add(items[i].Ship.Clone());
            }
            return list;
        }


        public void Add([NotNull] ShipListData shipData)
        {
            items.Add(shipData);
        }
    }
}
