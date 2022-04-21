using System.Collections.Generic;
using Core.Game;
using Core.UI;
using UnityEngine;
using Random = System.Random;

namespace Core.Garage
{
    public class GarageTradeExplorer : MonoBehaviour
    {
        private TradeManager manager;
        [SerializeField] private Transform holder, item;
        private List<GarageTradeItem> items = new List<GarageTradeItem>();
        private void Awake()
        {
            manager = FindObjectOfType<TradeManager>();
            manager.OnUpdateOffers += UpdateAll;
        }

        private void Start()
        {
            GarageDataCollect.Instance.cargo.OnChangeInventory += Redraw;
        }

        public void Redraw()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].ReInit();
            }
        }
        
        void UpdateAll()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].gameObject.SetActive(true);
            }
            items = new List<GarageTradeItem>();
            var seed = new Random(GarageDataCollect.Instance.stationSeed);
            UITweaks.ClearHolder(holder);
            for (int i = 0; i < manager.offers.Count; i++)
            {
                var isIn = seed.Next(0, 2);
                if (isIn == 0 || manager.offers[i].item.IsHaveKeyPair(KeyPairValue.Mineral))
                {
                    var it = Instantiate(item.gameObject, holder);
                    it.GetComponent<GarageTradeItem>().Init(manager.offers[i]);
                    it.gameObject.SetActive(true);
                    items.Add(it.GetComponent<GarageTradeItem>());
                }
            }
        }
        
        
        public void SetSorting(int type)
        {
            var key = (ItemType) type;

            for (int i = 0; i < items.Count; i++)
            {
                items[i].gameObject.SetActive(items[i].Compare(key));
            }
        }

        public void SetSortingHave(int keyPair)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (!items[i].IsHavePair((KeyPairValue) keyPair))
                {
                    items[i].gameObject.SetActive(false);
                }
            }
        }
        
        public void SetSortingByKeysCount(int keyCount)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (keyCount != items[i].GetKeysCount())
                {
                    items[i].gameObject.SetActive(false);
                }
            }
        }

    }
}
