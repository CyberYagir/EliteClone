using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "Game/ShipList", order = 1)]
public class ShipList : ScriptableObject
{
    [System.Serializable]
    public class ShipListData
    {
        public ItemShip ship;
        public ValueLimit cost;
    }
    [SerializeField] private List<ShipListData> items = new List<ShipListData>();

    public void SetList(List<ItemShip> newList)
    {
        for (int i = 0; i < newList.Count; i++)
        {
            var find = items.Find(x => x.ship.shipName == newList[i].shipName);
            if (find == null)
            {
                items.Add(new ShipListData() {ship = newList[i], cost = new ValueLimit()});
            }
        }
    }

    public ItemShip Get(string idName) => items.Find(x => x.ship.shipName == idName).ship.Clone();
    public ItemShip Get(ItemShip original) => items.Find(x => x.ship == original).ship.Clone();

    public ShipListData GetData(string idName) => items.Find(x => x.ship.shipName == idName);
    
    public List<ItemShip> GetShipsList()
    {
        var list = new List<ItemShip>();
        for (int i = 0; i < items.Count; i++)
        {
            list.Add(items[i].ship);
        }
        return list;
    }
}
