using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class GarageExplorer : CustomAnimate, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Transform holder, item;
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
        UITweaks.ClearHolder(holder);
        foreach (var it in GarageDataCollect.Instance.cargo.items)
        {
            var spawned = Instantiate(item, holder).GetComponent<GarageDragDropItem>();
            spawned.Init(it.icon, it);
            spawned.SetSprite(it.icon);
            spawned.gameObject.SetActive(true);
        }
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
    }
}
