using System;
using Core.Galaxy;
using UnityEngine;
using Random = System.Random;

namespace Core.Systems
{
    public class StructureCreator : MonoBehaviour
    {
        [SerializeField] private GameObject sun, hole, dysonSphere;


        private void Awake()
        {
            if (PlayerDataManager.CurrentSolarSystem != null && PlayerDataManager.CurrentSolarSystem.stars.Count == 1)
            {
                if (PlayerDataManager.CurrentSolarSystem.stars[0].starType == Star.StarType.Hole)
                {
                    sun.SetActive(false);
                    hole.SetActive(true);
                }
                else if (new Random((int)PlayerDataManager.CurrentSolarSystem.position.ToVector().magnitude * 10000).Next(0, 5) == 1)
                {
                    dysonSphere.gameObject.SetActive(true);
                }
            }
        }
    }
}
