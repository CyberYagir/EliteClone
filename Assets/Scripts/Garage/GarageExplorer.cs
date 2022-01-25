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
    }

    public void UpdateList()
    {
        UITweaks.ClearHolder(holder);
        foreach (var it in GarageDataCollect.Instance.playerData.items)
        {
            var spawned = Instantiate(item, holder).GetComponent<GarageDragDropItem>();
            var findedItem = ItemsManager.GetItem(it);
            spawned.Init(findedItem.icon, findedItem);
            spawned.SetSprite(findedItem.icon);
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
