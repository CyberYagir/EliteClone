using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GarageEnergyExplorer : MonoBehaviour
{
    [SerializeField] private Image allPowerI, usedPowerI;

    private void Start()
    {
        GarageDataCollect.OnChangeShip += CalculatePower;
    }

    public void CalculatePower()
    {
        GarageDataCollect.Instance.ship.OnChangeShipData += CalculatePower;
        
        var ship = GarageDataCollect.Instance.ship;
        var usedPower = 0f;
        var allPower = 0f;
        
        
        
        for (int i = 0; i < ship.slots.Count; i++)
        {
            if (ship.slots[i].current)
            {
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
}
