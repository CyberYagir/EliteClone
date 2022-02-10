using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GarageEnergyExplorer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image allPowerI, usedPowerI;
    [SerializeField] private GarageDrawEnergyItem item;
    [SerializeField] private RectTransform holder;
    [SerializeField] private RectTransform header, mask;
    private bool isAddChange = false;

    private bool over;
    private void Awake()
    {
        GarageDataCollect.OnChangeShip += CalculatePower;
        GarageDataCollect.OnChangeShip += AddChangeSlotsEvent;
    }
    
    private void Start()
    {
        GarageDataCollect.Instance.cargo.OnChangeInventory += CalculatePower;
    }

    public void AddChangeSlotsEvent()
    {
        GarageDataCollect.Instance.ship.OnChangeShipData += CalculatePower;
    }

    public void CalculatePower()
    {
        if (!isAddChange)
        {
            GarageDataCollect.Instance.ship.OnChangeShipData += CalculatePower;
            isAddChange = true;
        }

        var ship = GarageDataCollect.Instance.ship;
        var usedPower = 0f;
        var allPower = 0f;
        header.transform.parent = null;
        UITweaks.ClearHolder(holder);
        header.transform.parent = holder;
        
        for (int i = 0; i < ship.slots.Count; i++)
        {
            if (ship.slots[i].current)
            {

                var it = Instantiate(item.gameObject, holder).GetComponent<GarageDrawEnergyItem>();
                it.Init(ship.slots[i].current);
                it.gameObject.SetActive(true);
                
                
                if (ship.slots[i].current.itemType != ItemType.Generator)
                {
                    usedPower += Mathf.Abs((float) ship.slots[i].current.GetKeyPair(KeyPairValue.Energy));
                }
                else
                {
                    allPower += Mathf.Abs((float) ship.slots[i].current.GetKeyPair(KeyPairValue.Energy));
                }
            }
        }
        
        if (usedPower > allPower)
        {
            allPowerI.transform.DOScale(new Vector3((allPower / usedPower), 1, 1), 1f);
            usedPowerI.transform.DOScale(Vector3.one, 1f);
        }
        else if (usedPower < allPower)
        {
            allPowerI.transform.DOScale(Vector3.one, 1f);
            usedPowerI.transform.DOScale(new Vector3((usedPower / allPower), 1, 1), 1f);
        }
        else if (usedPower == 0 && allPower == 0)
        {
            allPowerI.transform.DOScale(new Vector3(0, 1f, 1f), 1f);
            usedPowerI.transform.DOScale(new Vector3(0, 1f, 1f), 1f);
        }
        else
        {
            allPowerI.transform.DOScale(Vector3.one, 1f);
            usedPowerI.transform.DOScale(Vector3.one, 1f);
        }
    }

    private void Update()
    {
        if (over)
        {
            mask.sizeDelta = Vector2.Lerp(mask.sizeDelta, new Vector2(mask.sizeDelta.x, holder.sizeDelta.y), 10 * Time.deltaTime);
        }
        else
        {
            mask.sizeDelta = Vector2.Lerp(mask.sizeDelta, new Vector2(mask.sizeDelta.x, 0), 10 * Time.deltaTime);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
    }
}
