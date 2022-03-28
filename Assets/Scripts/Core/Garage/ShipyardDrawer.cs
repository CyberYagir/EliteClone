using System.Collections.Generic;
using Core.Game;
using Core.UI;
using UnityEngine;

namespace Core.Garage
{
    public class ShipyardDrawer : ShipyardShipsList
    {
        [SerializeField] private Transform holder, item;

        public List<ItemShip> GetShips()
        {
            List<ItemShip> newList = new List<ItemShip>();
            var rnd = new System.Random(GarageDataCollect.Instance.stationSeed);
            var list = ItemsManager.GetShisList();
            for (int i = 0; i < list.Count; i++)
            {
                if (rnd.Next(0, 2) != 0)
                {
                    newList.Add(list[i].Clone());
                }
            }

            return newList;
        }
    
        private void Start()
        {
            UITweaks.ClearHolder(holder);
            var ships = GetShips();

            for (int i = 0; i < ships.Count; i++)
            {
                var it = Instantiate(item, holder);
                it.GetComponent<ShipyardItem>().Init(ships[i], true);
            }
        }
    }
}
