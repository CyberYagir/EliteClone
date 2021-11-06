using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyManager : MonoBehaviour
{
    public static GalaxyPoint selectedPoint { get; private set; }
    public static event Action onUpdateSelected = delegate { };
    private void Awake()
    {
        onUpdateSelected = delegate { };
    }

    private void Start()
    {
        Application.targetFrameRate = 120;
    }

    private void Update()
    {
        if (selectedPoint != null)
        {
            selectedPoint.particles.Play();
        }
    }

    public static void Select(GalaxyPoint newSel)
    {
        if (newSel != selectedPoint)
        {
            selectedPoint = newSel;
            onUpdateSelected();
        }
    }


    public static void JumpToSolarSystem()
    {
        PlayerDataManager.currentSolarSystem = selectedPoint.solarSystem;
        Application.LoadLevel(1);
    }
}
