using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class GarageGenerator : MonoBehaviour
{
    [SerializeField] private GarageDataCollect dataCollect;
    private void Start()
    {
        dataCollect.InitDataCollector();
    }
}
