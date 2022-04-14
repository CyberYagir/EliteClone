using System;
using System.Collections;
using System.Collections.Generic;
using Core.Galaxy;
using UnityEngine;

namespace Core.Systems
{
    public class BackHoleCreator : MonoBehaviour
    {
        [SerializeField] private GameObject sun, hole;


        private void Awake()
        {
            if (PlayerDataManager.CurrentSolarSystem != null && PlayerDataManager.CurrentSolarSystem.stars.Count == 1)
            {
                if (PlayerDataManager.CurrentSolarSystem.stars[0].starType == Star.StarType.Hole)
                {
                    print(PlayerDataManager.CurrentSolarSystem.stars[0].radius * SolarSystemGenerator.scale);
                    sun.SetActive(false);
                    hole.SetActive(true);
                }
            }
        }
    }
}
