using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "", menuName = "Game/PlanetTextures", order = 1)]
public class PlanetTextures : ScriptableObject
{
    [System.Serializable]
    public class PlanetMaterial
    {
        public Planet.PlanetType type;
        public Material material;
    }
    public List<PlanetMaterial> textures = new List<PlanetMaterial>();
}
