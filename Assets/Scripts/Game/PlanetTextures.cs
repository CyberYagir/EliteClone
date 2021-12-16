using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(fileName = "", menuName = "Game/PlanetTextures", order = 1)]
public class PlanetTextures : ScriptableObject
{
    public List<Material> textures = new List<Material>();
}
