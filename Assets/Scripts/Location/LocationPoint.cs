using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationPoint : MonoBehaviour
{
    private Camera camera;
    [SerializeField] float size;
    public GameObject root;
    public float minDist;
    private void Start()
    {
        camera = Camera.main;
    }
    void Update()
    {
        transform.LookAt(camera.transform);
        transform.localScale = Vector3.one * Vector3.Distance(transform.position, camera.transform.position) * size;

        SetActiveLocation();
    }

    public void SetActiveLocation()
    {
        if (Vector3.Distance(transform.position, camera.transform.position) * SolarSystemGenerator.scale < minDist * SolarSystemGenerator.scale)
        {
            Player.inst.warp.SetActiveLocation(this);
        }
        else
        {
            Player.inst.warp.RemoveActiveLocation(this);
        }
    }
}
