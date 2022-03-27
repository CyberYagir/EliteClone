using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decal : MonoBehaviour
{
    [SerializeField] private Renderer renderer;
    [SerializeField] private float opacity = 0.5f;

    public void AddToOpacity()
    {
        if (opacity < 1)
        {
            opacity += 0.1f;
            renderer.material.SetFloat("_DecalBlend", opacity);
        }
    }
}
