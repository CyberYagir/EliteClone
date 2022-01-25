using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GarageItemInfo : CustomAnimate, IPointerEnterHandler, IPointerExitHandler
{
    [System.Serializable]
    private class GarageItemInfoUIData
    {
        public Image image;
        public TMP_Text itemName, itemData, itemValue, itemType;
        public ItemReplacer replacer;
    }
    
    [SerializeField] private GarageItemInfoUIData options;
    
    private GarageSlotInfo slotInfo;
    private Item currentItem;
    private Slot currentSlot;
    private void Awake()
    {
        slotInfo = FindObjectOfType<GarageSlotInfo>();
    }

    private void Start()
    {
        Init();
    }

    public void Update()
    {
        if (currentSlot != null && currentItem != null && currentItem.itemType == currentSlot.slotType)
        {
            options.replacer.enabled = true;
        }
        else
        {
            options.replacer.Disable();
        }
    }

    public void SetItem(Item item)
    {
        currentItem = item;
        currentSlot = null;
        slotInfo.CloseObject();
        UpdateAll();
    }

    public void ManageItem()
    {
        ItemReplacer.SetSelected(slotInfo.lastItem.slot);
        currentItem = slotInfo.lastItem.slot.current;
        currentSlot = slotInfo.lastItem.slot;
        UpdateAll();
    }
    

    public void UpdateAll()
    {
        options.itemName.text = currentItem.itemName;
        if (slotInfo.lastItem != null)
        {
            options.itemName.text += $" [Slot {slotInfo.lastItem.slot.uid}]";
        }
        options.itemData.text = "Data: \n";
        if (currentItem.id.idname != "credit")
        {
            options.itemValue.text = currentItem.amount.Value + "/" + currentItem.amount.Max;
        }
        else
        {
            options.itemValue.text = currentItem.amount.Value.ToString("F2");
        }

        if (currentItem.itemType != ItemType.None)
        {
            options.itemType.text = currentItem.itemType.ToString();
        }
        else
        {
            options.itemType.text = "";
        }

        options.image.sprite = currentItem.icon;
        for (int i = 0; i < currentItem.keysData.Count; i++)
        {
            var key = currentItem.keysData[i];
            options.itemData.text += (key.customName == "" ? key.KeyPairValue.ToString() : key.customName) + ": " + (key.KeyPairType == KeyPairType.String ? key.str : key.num.ToString(key.KeyPairType == KeyPairType.Int ? "" : "F2")) + "\n";
        }
    }
    

   
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (slotInfo.lastItem != null && currentItem == slotInfo.lastItem.slot.current)
        {
            slotInfo.over = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (slotInfo.lastItem == null || currentItem == slotInfo.lastItem.slot.current)
        {
            slotInfo.over = false;
        }
    }
}
