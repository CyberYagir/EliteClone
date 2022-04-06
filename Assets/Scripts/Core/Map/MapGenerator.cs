using System;
using System.Collections;
using System.Collections.Generic;
using Core.Galaxy;
using UnityEngine;

namespace Core.Map
{
    public class MapGenerator : MonoBehaviour
    {
        [SerializeField] private SaveLoadData saveload;
        [SerializeField] private GameObject star;
        private void Start()
        {
            GalaxyGenerator.LoadSystems();
            saveload.LoadData();
            foreach (var history in saveload.GetHistory())
            {
                var system = GalaxyGenerator.systems[history.Split('.')[0]];

                Instantiate(star.gameObject, system.position.ToVector()/2000, Quaternion.identity).SetActive(true);
            }
        }
    }
}
