using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decal : MonoBehaviour
{
    [SerializeField] private new Renderer renderer;
    [SerializeField] private float opacity = 0.5f;
    private static readonly int DecalBlend = Shader.PropertyToID("_DecalBlend");

    public void AddToOpacity()
    {
        if (opacity < 1)
        {
            opacity += 0.1f;
            renderer.material.SetFloat(DecalBlend, opacity);
        }
    }
}
