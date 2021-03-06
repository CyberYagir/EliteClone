using UnityEngine;

namespace Core.Systems
{
    public abstract class TexturingScript: MonoBehaviour{}


    public class PlanetTexture : TexturingScript
    {
        [SerializeField] private Renderer[] renderers;
        public int setted;
        public void SetTexture(int id)
        {
            if (SolarSystemGenerator.planetTextures == null)
            {
                SolarSystemGenerator.GetPlanetTextures();
            }

            setted = id;
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = SolarSystemGenerator.planetTextures.textures[id].material;
            }
        
        }
        public int GetLen()
        {
            return SolarSystemGenerator.planetTextures.textures.Count;
        }
    }
}