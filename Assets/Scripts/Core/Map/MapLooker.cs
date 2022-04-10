using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core
{
    public class MapLooker : MonoBehaviour
    {
        [SerializeField] private Camera camera;


        void Update()
        {
            transform.LookAt(camera.transform);
        }
    }
}
