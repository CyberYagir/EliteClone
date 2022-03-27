using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class GarageExplorer : CustomAnimate, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform holder, item;
    [SerializeField] private StatsDisplayCanvasRow row;
    private GarageSlotInfo slotInfo;
    private GarageItemInfo itemInfo;
    void Start()
    {
        slotInfo = FindObjectOfType<GarageSlotInfo>();
        itemInfo = FindObjectOfType<GarageItemInfo>();
        Init();
        UpdateList();
        GarageDataCollect.OnChangeShip += UpdateList;
        GarageDataCollect.Instance.ship.OnChangeShipData += UpdateList;
        GarageDataCollect.Instance.cargo.OnChangeInventory += UpdateList;
    }

    public void UpdateList()
    {
        UITweaks.ClearHolderAll(holder);
        foreach (var it in GarageDataCollect.Instance.cargo.items)
        {
            var spawned = Instantiate(item, holder).GetComponent<GarageDragDropItem>();
            spawned.Init(it.icon, it);
            spawned.SetSprite(it);
            spawned.gameObject.SetActive(true);
        }
        row.SetValue(GarageDataCollect.Instance.cargo.tons, GarageDataCollect.Instance.ship.data.maxCargoWeight, $"Weight {GarageDataCollect.Instance.cargo.tons:F1}/{GarageDataCollect.Instance.ship.data.maxCargoWeight}");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotInfo == null) return;
        if (slotInfo.last != null)
        {
            slotInfo.over = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (slotInfo == null) return;
        if (slotInfo.last != null)
        {
            slotInfo.over = false;
        }
    }

    public void SetItem(Item data)
    {
        if (itemInfo == null) return;
        itemInfo.SetItem(data);
        itemInfo.Show();
    }
}
