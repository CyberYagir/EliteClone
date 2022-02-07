using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using Random = System.Random;

public class GarageTradeExplorer : MonoBehaviour
{
    private TradeManager manager;
    [SerializeField] private Transform holder, item;
    private void Awake()
    {
        manager = FindObjectOfType<TradeManager>();
        manager.OnUpdateOffers += UpdateAll;
    }

    private void Start()
    {
        GarageDataCollect.Instance.cargo.OnChangeInventory += UpdateAll;
    }

    void UpdateAll()
    {
        var seed = new Random(GarageDataCollect.Instance.stationSeed);
        UITweaks.ClearHolder(holder);
        for (int i = 0; i < manager.offers.Count; i++)
        {
            var isIn = seed.Next(0, 2);
            if (isIn == 0)
            {
                var it = Instantiate(item.gameObject, holder);
                it.GetComponent<GarageTradeItem>().Init(manager.offers[i]);
                it.gameObject.SetActive(true);
            }
        }
    }
    
}
