using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationPoint : MonoBehaviour
{
    Camera camera;
    [SerializeField] float size;
    private void Start()
    {
        camera = Camera.main;
        transform.parent.localScale = Vector3.one;
    }
    void Update()
    {
        transform.LookAt(camera.transform);
        transform.localScale = Vector3.one * Vector3.Distance(transform.position, camera.transform.position) * size;
    }
}
