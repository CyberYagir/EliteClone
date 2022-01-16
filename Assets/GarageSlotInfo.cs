using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GarageSlotInfo : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private TMP_Text slotText, slotLevel, slotType;
    [SerializeField] private Image preview;
    [SerializeField] private bool close;
    
    private RectTransform rect;
    private float time;
    
    public bool over;
    public RectTransform last;
    public GarageSlotItem lastItem;
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (close && !over)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                rect.localScale = Vector3.Lerp(rect.localScale, Vector3.zero, Time.deltaTime * 10f);
                if (rect.localScale.magnitude < 0.1f)
                {
                    if (lastItem != null)
                    {
                        lastItem.Deselect();
                        lastItem = null;
                    }
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            lastItem.Select();
            time = 0;
        }
        if (gameObject.activeSelf)
        {
            var spacing = 50;
            var pos = last.position;
            rect.position = Vector3.Lerp(rect.position, pos, 5 * Time.deltaTime);
        }
    }

    public void Init(Slot slot, RectTransform postion, Sprite sprite, GarageSlotItem item)
    {
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }

        time = 0;
        close = false;
        slotText.text = $"Slot [{slot.uid}]";
        slotLevel.text = "Level: " + slot.slotLevel.ToString();
        slotType.text = "Type: " + slot.slotType.ToString();
        preview.sprite = sprite;
        last = postion;
        lastItem = item;
        var pos = new Vector3(last.anchoredPosition.x + last.sizeDelta.x + 20, last.anchoredPosition.y, 0);

        if (!gameObject.activeSelf)
        {
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, 0.5f);
            rect.anchoredPosition = pos;
            gameObject.SetActive(true);
        }
    }

    public void Close()
    {
        close = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }
}
