using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.CommunistsBase
{
    public class RandomObject : MonoBehaviour
    {
        [Range(0, 1)]
        public float chanse = 0.5f;

        private void Start()
        {
            gameObject.SetActive(Random.Range(0, 1f) <= chanse);
        }
    }
}
