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
            var id = new Random((int) PlayerDataManager.CurrentSolarSystem.position.ToVector().magnitude * 10000).Next(0, spheres.Count);
            for (int i = 0; i < spheres.Count; i++)
            {
                spheres[i].SetActive(i == id);
            }   
        }
    }
}
