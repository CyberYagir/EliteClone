using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTexture : MonoBehaviour
{
    [SerializeField] Material[] textures;

    public void SetTexture(int id)
    {
        GetComponent<Renderer>().material = textures[id];
    }
    public int GetLen()
    {
        return textures.Length;
    }
}
