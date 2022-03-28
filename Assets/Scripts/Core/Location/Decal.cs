using UnityEngine;

namespace Core.Location
{
    public class Decal : MonoBehaviour
    {
        [SerializeField] private Renderer mesh;
        [SerializeField] private float opacity = 0.5f;
        private static readonly int DecalBlend = Shader.PropertyToID("_DecalBlend");

        public void AddToOpacity()
        {
            if (opacity < 1)
            {
                opacity += 0.1f;
                mesh.material.SetFloat(DecalBlend, opacity);
            }
        }
    }
}
