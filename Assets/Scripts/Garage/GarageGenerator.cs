using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class GarageGenerator : MonoBehaviour
{
    public static GarageGenerator Instance;
    [SerializeField] private GarageDataCollect dataCollect;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        dataCollect.InitDataCollector();
    }
}
