using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.CommunistsBase
{
    public class ShipRandom : MonoBehaviour
    {
        public List<Transform> meshes;

        private void Start()
        {
            var isHaveShip = Random.Range(0f, 1f);
            if (isHaveShip <= 0.5f)
            {
                meshes[Random.Range(0, meshes.Count)].gameObject.SetActive(true);
            }
        }
    }
}
