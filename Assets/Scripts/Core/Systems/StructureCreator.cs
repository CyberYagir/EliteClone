using System;
using Core.Galaxy;
using UnityEngine;
using Random = System.Random;

namespace Core.Systems
{
    public class StructureCreator : MonoBehaviour
    {
        [SerializeField] private GameObject sun, hole, dysonSphere;

        private WorldDataHandler worldHandler;

        private void Awake()
        {
            worldHandler = PlayerDataManager.Instance.WorldHandler;
            
            CreateStructure();
        }

        private void CreateStructure()
        {
            if (worldHandler.CurrentSolarSystem != null && worldHandler.CurrentSolarSystem.stars.Count == 1)
            {
                if (worldHandler.CurrentSolarSystem.stars[0].starType == Star.StarType.Hole)
                {
                    sun.SetActive(false);
                    hole.SetActive(true);
                }
                else if (new Random((int) worldHandler.CurrentSolarSystem.position.ToVector().magnitude * 10000).Next(0, 5) == 1)
                {
                    dysonSphere.gameObject.SetActive(true);
                }
            }
        }
    }
}
