using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationPoint : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField] float size;
    public GameObject root;
    public float minDist;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    void Update()
    {
        transform.LookAt(mainCamera.transform);
        transform.localScale = Vector3.one * Vector3.Distance(transform.position, mainCamera.transform.position) * size;

        SetActiveLocation();
    }

    public void SetActiveLocation()
    {
        if (Vector3.Distance(transform.position, mainCamera.transform.position) * SolarSystemGenerator.scale < minDist * SolarSystemGenerator.scale)
        {
            Player.inst.warp.SetActiveLocation(this);
        }
        else
        {
            Player.inst.warp.RemoveActiveLocation(this);
        }
    }
}
