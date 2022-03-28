using System.Collections.Generic;
using Core.Galaxy;
using UnityEngine;


namespace Core.Game
{
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
}
