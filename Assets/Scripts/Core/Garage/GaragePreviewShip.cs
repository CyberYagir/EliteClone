using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Garage
{
    public class GaragePreviewShip : MonoBehaviour
    {
        [SerializeField] private GameObject mesh;
        [SerializeField] private GameObject spaceMesh;

        public void CheckIsMesh(bool active)
        {
            mesh.SetActive(active);
            spaceMesh.SetActive(!active);
        }
    }
}
