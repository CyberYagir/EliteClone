using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTexture : MonoBehaviour
{
    [SerializeField] Material[] textures;

    public void SetTexture(System.Random rnd)
    {
        GetComponent<Renderer>().material = textures[rnd.Next(0, textures.Length)];
    }
}
