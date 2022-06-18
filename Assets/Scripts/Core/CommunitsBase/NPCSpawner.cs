using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.CommunistsBase
{
    public class NPCSpawner : MonoBehaviour
    {
        [SerializeField] private int count;
        [SerializeField] private GameObject[] npcs;
        [SerializeField] private List<Transform> spawnpoints;
        private void Start()
        {
            for (int i = 0; i < count; i++)
            {
                Instantiate(npcs[Random.Range(0, npcs.Length)], spawnpoints[Random.Range(0, spawnpoints.Count)].position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)), Quaternion.identity, transform);
            }
        }
    }
}
