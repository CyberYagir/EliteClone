using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Init
{
    public class InitEnvRandom : MonoBehaviour
    {
        [SerializeField] private List<GameObject> environments;

        private void Awake()
        {
            var n = Random.Range(0, environments.Count);
            for (int i = 0; i < environments.Count; i++)
            {
                environments[i].SetActive(i == n);
            }
        }
    }
}
