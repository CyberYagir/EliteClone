using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Map
{
    public class MapRandomStar : MonoBehaviour
    {
        [SerializeField] private Renderer renderer;
        [SerializeField] private List<Texture2D> textures;
        private static readonly int MainTex = Shader.PropertyToID("_UnlitColorMap");
        private static readonly int Emission = Shader.PropertyToID("_EmissiveColorMap");

        private void Awake()
        {
            var tex = textures[new System.Random((int) (transform.position.magnitude * 100)).Next(0, textures.Count)];
            renderer.material.SetTexture(MainTex, tex);
            renderer.material.SetTexture(Emission, tex);
        }
    }
}
