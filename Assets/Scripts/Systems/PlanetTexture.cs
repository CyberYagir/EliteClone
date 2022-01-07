using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTexture : MonoBehaviour
{
    [SerializeField] private Renderer[] renderers;
    public void SetTexture(int id)
    {
        if (SolarSystemGenerator.planetTextures == null)
        {
            SolarSystemGenerator.GetPlanetTextures();
        }

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material = SolarSystemGenerator.planetTextures.textures[id];
        }
        
    }
    public int GetLen()
    {
        return SolarSystemGenerator.planetTextures.textures.Count;
    }
}
