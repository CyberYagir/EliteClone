using Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Garage
{
    public class GarageTradeItem : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameT, costT, haveT;
        private TradeManager.Offer offer;

        public void ReInit()
        {
            Init(offer);
        }
        public void Init(TradeManager.Offer item)
        {
            offer = item;
            image.sprite = item.item.icon;
            nameT.text = item.item.itemName;
            costT.text = item.cost.ToString();
            var count = 0;
            var allItem = GarageDataCollect.Instance.cargo.items.FindAll(x => x.id.idname == item.item.id.idname);
            for (int i = 0; i < allItem.Count; i++)
            {
                count += (int)allItem[i].amount.Value;
            }

            if (!offer.item.IsHaveKeyPair(KeyPairValue.Mineral))
            {
                haveT.text = "INV: " + count;
            }
            else
            {
                haveT.text = "INV: " + (count/100);
            }
        }

        public void BuyOne()
        {
            if (!offer.item.IsHaveKeyPair(KeyPairValue.Mineral))
            {
                if (GarageDataCollect.Instance.cargo.RemoveCredits(offer.cost))
                {
                    if (!GarageDataCollect.Instance.cargo.AddItem(offer.item.Clone()))
                    {
                        GarageDataCollect.Instance.cargo.AddCredits(offer.cost);
                    }
                }
            }
            else
            {
                if (GarageDataCollect.Instance.cargo.RemoveCredits(offer.cost*100))
                {
                    var item = offer.item.Clone();
                    item.amount.value = item.amount.Max;
                    if (!GarageDataCollect.Instance.cargo.AddItem(item))
                    {
                        GarageDataCollect.Instance.cargo.AddCredits(offer.cost*100);
                    }
                }
            }
        }

        public void SellOne()
        {
            var allItem = GarageDataCollect.Instance.cargo.items.FindAll(x => x.id.idname == offer.item.id.idname);
            if (allItem.Count > 0)
            {
                if (!offer.item.IsHaveKeyPair(KeyPairValue.Mineral))
                {
                    if (GarageDataCollect.Instance.cargo.RemoveItem(offer.item.id.idname, 1, true))
                    {
                        GarageDataCollect.Instance.cargo.AddCredits(offer.cost);
                    }
                }
                else
                {
                    if (allItem[0].amount.value == allItem[0].amount.Max)
                    {
                        if (GarageDataCollect.Instance.cargo.RemoveItem(offer.item.id.idname, 100, true))
                        {
                            GarageDataCollect.Instance.cargo.AddCredits(offer.cost * 100);
                        }
                    }
                }
            }
        }
        
        public bool Compare(ItemType key)
        {
            return offer.item.itemType == key;
        }

        public bool IsHavePair(KeyPairValue key)
        {
            return offer.item.IsHaveKeyPair(key);
        }

        public int GetKeysCount()
        {
            return offer.item.keysData.Count;
        }

    }
}
