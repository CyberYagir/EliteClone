using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceObject : MonoBehaviour
{
    public bool isVisible;    
    [HideInInspector]
    public string dist;
    private Transform camera;
    public Sprite icon;
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
