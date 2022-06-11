using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.CommunistsBase
{
    public class NPCColor : MonoBehaviour
    {
        [SerializeField] private Renderer renderer;
        [SerializeField] private Material[] mats;
        [SerializeField] private int matId;

        private void Start()
        {
            var list = new List<Material>(3);
            renderer.GetMaterials(list);
            list[matId] = mats[Random.Range(0, mats.Length)];
            renderer.materials = list.ToArray();
        }
    }
}
