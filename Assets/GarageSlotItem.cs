using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GarageSlotItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private RectTransform content;
    [SerializeField] private RectTransform select, point;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    [SerializeField] private GarageSlotInfo slotInfo;
    
    [SerializeField] public Slot slot;
    public bool over;
    [SerializeField] private float minX, maxX;

    public void Init(Item item, Slot slt)
    {
        slot = slt;
        text.text = item.itemName;
    }

    private void FixedUpdate()
    {
        if (slotInfo.last != point)
        {
            Deselect();
        }
    }

    private void Start()
    {
        select.transform.localScale = new Vector3(0, 1, 1);
    }

    public void SetImage(Sprite img)
    {
        image.sprite = img;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        slotInfo.Init(slot, point, image.sprite, this);
        Select();
    }

    public void Select()
    {
        content.DOLocalMove(new Vector3(maxX, content.localPosition.y, content.localPosition.z), 0.2f);
        select.DOScale(new Vector3(1, 1, 1), 0.5f);
        over = true;
    }

    public void Deselect()
    {
        content.DOLocalMove(new Vector3(minX, content.localPosition.y, content.localPosition.z), 0.2f);
        select.DOScale(new Vector3(0, 1, 1), 0.5f);
        over = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotInfo.Close();
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
