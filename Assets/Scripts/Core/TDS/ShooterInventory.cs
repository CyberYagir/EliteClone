using System.Collections;
using System.Collections.Generic;
using Core.Game;
using UnityEngine;

namespace Core.TDS
{
    public class ShooterInventory : MonoBehaviour
    {
        public List<Item> items;
        public Event OnChange = new Event();

        public void Add(Item item)
        {
            if (items.Find(x => x.id.id == item.id.id) == null)
            {
                items.Add(item);
                OnChange.Run();
            }
        }
        
    }

}
