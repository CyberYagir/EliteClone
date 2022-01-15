using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "Game/ShipList", order = 1)]
public class ShipList : ScriptableObject
{
    [SerializeField] private List<ItemShip> items = new List<ItemShip>();

    public void SetList(List<ItemShip> newList)
    {
        items = newList;
    }

    public ItemShip Get(string idName) => items.Find(x => x.shipName == idName).Clone();
    public ItemShip Get(ItemShip original) => items.Find(x => x == original).Clone();
}
