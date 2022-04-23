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

        private Dictionary<ItemType, int> slotLevels = new Dictionary<ItemType, int>();
        private void Awake()
        {
            manager = FindObjectOfType<TradeManager>();
            manager.OnUpdateOffers += UpdateAll;
            GarageDataCollect.OnChangeShip += UpdateAll;
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

        public void CalcMaxes()
        {
            slotLevels = new Dictionary<ItemType, int>();
            if (GarageDataCollect.Instance.ship)
            {
                foreach (var slot in GarageDataCollect.Instance.ship.slots)
                {
                    if (!slotLevels.ContainsKey(slot.slotType))
                    {
                        slotLevels.Add(slot.slotType, slot.slotLevel);
                    }

                    if (slotLevels[slot.slotType] > slot.slotLevel)
                    {
                        slotLevels[slot.slotType] = slot.slotLevel;
                    }
                }
            }
        }
        
        void UpdateAll()
        {
            CalcMaxes();
            
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
                    var gti = it.GetComponent<GarageTradeItem>();
                    gti.Init(manager.offers[i]);
                    it.gameObject.SetActive(true);
                    items.Add(gti);

                    if (!manager.offers[i].item.IsHaveKeyPair(KeyPairValue.Mineral))
                    {
                        if (slotLevels.ContainsKey(manager.offers[i].item.itemType))
                        {
                            if ((float) manager.offers[i].item.GetKeyPair(KeyPairValue.Level) > slotLevels[manager.offers[i].item.itemType])
                            {
                                gti.SetBorderColor(Color.gray);
                            }
                        }
                    }
                }
            }
        }
        
        
        public void SetSorting(int type)
        {
            holder.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
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
