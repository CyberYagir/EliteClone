using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetTexture : MonoBehaviour
{
    public void SetTexture(int id)
    {
        if (SolarSystemGenerator.planetTextures == null)
        {
            SolarSystemGenerator.GetPlanetTextures();
        }

        GetComponent<Renderer>().material = SolarSystemGenerator.planetTextures.textures[id];
    }
    public int GetLen()
    {
        return SolarSystemGenerator.planetTextures.textures.Count;
    }
}
