using UnityEngine;

namespace Core.Galaxy
{
    public class GalaxyPoint : MonoBehaviour
    {
        public SolarSystem solarSystem;
        public ParticleSystem particles;
        [SerializeField] ParticleSystemRenderer particlesRenderer;
        [SerializeField] Texture2D[] textures;
        int texture;
        private void Start()
        {
            particles.startColor = solarSystem.stars[0].GetColor();
            texture = Random.Range(0, textures.Length);
            particlesRenderer.material.mainTexture = textures[texture];
        }
        public Texture2D GetTexture()
        {
            return textures[texture];
        }

        private void OnMouseDown()
        {
            if (GalaxyManager.Select(this))
            {
                if (LineDrawer.Instance.stars.Contains(this))
                {
                    LineDrawer.Instance.stars.Remove(this);
                }

                LineDrawer.Instance.stars.Insert(0, this);
                LineDrawer.Instance.UpdateLines();
            }
        }
    }
}
