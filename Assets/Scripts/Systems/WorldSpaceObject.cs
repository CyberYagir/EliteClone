using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GalaxyObject : MonoBehaviour
{
    public Sprite icon;
}

public class WorldSpaceObject : GalaxyObject
{
    public bool isVisible;    
    [HideInInspector]
    public string dist;
    private Transform camera;
    private void Start()
    {
        camera = Camera.main.transform;
    }

    private void Update()
    {
        UpdateVisibility();
    }

    public void UpdateVisibility()
    {
        isVisible =  Vector3.Angle(transform.position - camera.transform.position, camera.forward) < 60;
    }

    private void OnDestroy()
    {
        if (SolarSystemGenerator.objects != null)
        {
            SolarSystemGenerator.objects.Remove(this);
        }
    }
}
