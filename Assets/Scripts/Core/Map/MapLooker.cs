using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class MapLooker : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private float scale;

        void Update()
        {
            transform.LookAt(camera.transform);
            transform.localScale = Vector3.one * Vector3.Distance(transform.position, camera.transform.position) * scale;
        }
    }
}
