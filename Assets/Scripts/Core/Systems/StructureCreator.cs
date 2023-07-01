using System;
using Core.Core.Inject.FoldersManagerService;
using Core.Core.Inject.GlobalDataService;
using Core.Galaxy;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Core.Systems
{
    public class StructureCreator : MonoBehaviour
    {
        [SerializeField] private GameObject sun, hole, dysonSphere;


        [Inject]
        private void Constructor(SolarSystemService solarSystemService)
        {
            if (solarSystemService.CurrentSolarSystem != null && solarSystemService.CurrentSolarSystem.stars.Count == 1)
            {
                if (solarSystemService.CurrentSolarSystem.stars[0].starType == Star.StarType.Hole)
                {
                    sun.SetActive(false);
                    hole.SetActive(true);
                }
                else if (new Random((int)solarSystemService.CurrentSolarSystem.position.ToVector().magnitude * 10000).Next(0, 5) == 1)
                {
                    dysonSphere.gameObject.SetActive(true);
                }
            }
        }
    }
}
