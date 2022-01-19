using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GarageItemInfo : CustomAnimate, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private Image image;
    [SerializeField]private TMP_Text itemName, itemData, itemValue, itemType;
    [SerializeField] private GarageSlotInfo info;
    private Item currentItem;

    private void Start()
    {
        Init();
    }

    public void Init(Item item)
    {
        currentItem = item;
        UpdateAll();
    }

    public void ManageItem()
    {
        currentItem = info.lastItem.slot.current;
        UpdateAll();
    }
    

    public void UpdateAll()
    {
        itemName.text = currentItem.itemName;
        itemData.text = "Data: \n";
        itemValue.text = currentItem.amount.Value + "/" + currentItem.amount.Max;
        itemType.text = currentItem.itemType.ToString();
        image.sprite = currentItem.icon;
        for (int i = 0; i < currentItem.keysData.Count; i++)
        {
            var key = currentItem.keysData[i];
            itemData.text += (key.customName == "" ? key.KeyPairValue.ToString() : key.customName) + ": " + (key.KeyPairType == KeyPairType.String ? key.str : key.num.ToString(key.KeyPairType == KeyPairType.Int ? "" : "F2")) + "\n";
        }
    }
    

   
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        info.over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        info.over = false;
    }
}
