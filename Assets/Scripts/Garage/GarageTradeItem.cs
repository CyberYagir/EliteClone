using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GarageTradeItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text nameT, costT, haveT;
    private TradeManager.Offer offer;
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

        haveT.text = "INV: " + count;
    }

    public void BuyOne()
    {
        if (GarageDataCollect.Instance.cargo.RemoveCredits(offer.cost))
        {
            if (!GarageDataCollect.Instance.cargo.AddItem(offer.item.Clone()))
            {
                GarageDataCollect.Instance.cargo.AddCredits(offer.cost);
            }
        }
    }

    public void SellOne()
    {
        var allItem = GarageDataCollect.Instance.cargo.items.FindAll(x => x.id.idname == offer.item.id.idname);
        if (allItem.Count > 0)
        {
            if (GarageDataCollect.Instance.cargo.RemoveItem(offer.item.id.idname, 1, true))
            {
                GarageDataCollect.Instance.cargo.AddCredits(offer.cost);
            }
        }
    }
}
