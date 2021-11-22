using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceObject : MonoBehaviour
{
    [HideInInspector]
    public bool isVisible;    
    [HideInInspector]
    public string dist;
    Transform camera;
    public Sprite icon;
    private void Start()
    {
        camera = Camera.main.transform;
    }

    private void Update()
    {
        isVisible = Vector3.Angle(transform.position - camera.transform.position, camera.forward) < 60;
    }
}
