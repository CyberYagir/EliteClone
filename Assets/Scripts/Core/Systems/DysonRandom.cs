using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Core.Systems
{
    public class DysonRandom : MonoBehaviour
    {
        [SerializeField] private List<GameObject> spheres;
        private void Start()
        {
            WorldDataHandler worldDataHandler = PlayerDataManager.Instance.WorldHandler;
            
            
            var id = new Random((int) worldDataHandler.CurrentSolarSystem.position.ToVector().magnitude * 10000).Next(0, spheres.Count);
            for (int i = 0; i < spheres.Count; i++)
            {
                spheres[i].SetActive(i == id);
            }   
        }
    }
}
