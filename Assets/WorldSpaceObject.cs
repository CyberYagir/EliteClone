using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceObject : MonoBehaviour
{
    public bool isVisible;
    public string dist;
    Transform camera;

    private void Start()
    {
        camera = Camera.main.transform;
    }

    private void Update()
    {
        isVisible = Vector3.Angle(transform.position - camera.transform.position, camera.forward) < 60;
    }
}
