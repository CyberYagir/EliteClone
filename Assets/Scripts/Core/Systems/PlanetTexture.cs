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
            if (SolarStaticBuilder.PlanetTextures == null)
            {
                SolarStaticBuilder.GetPlanetTextures();
            }

            setted = id;
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].material = SolarStaticBuilder.PlanetTextures.textures[id].material;
            }
        
        }
        public int GetLen()
        {
            return SolarStaticBuilder.PlanetTextures.textures.Count;
        }
    }
}