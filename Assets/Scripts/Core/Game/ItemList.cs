using System.Collections.Generic;
using UnityEngine;

namespace Core.Game
{
    [CreateAssetMenu(fileName = "", menuName = "Game/ItemList", order = 1)]
    public class ItemList : ScriptableObject
    {
        [SerializeField] private List<Item> items = new List<Item>();

        public void SetList(List<Item> newList)
        {
            items = newList;
        }

        public Item Get(string idName) => items.Find(x => x.id.idname == idName.Trim().ToLower().Replace(" ", "_")).Clone();
        public Item Get(Item original) => items.Find(x => x == original).Clone();
        public Item Get(int id) => items.Find(x => x.id.id == id).Clone();

        public List<Item> GetItemList()
        {
            var list = new List<Item>();
            for (int i = 0; i < items.Count; i++)
            {
                list.Add(items[i]);
            }

            return list;
        }
    }
}
