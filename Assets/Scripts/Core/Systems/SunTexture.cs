using System.Collections.Generic;
using Core.Galaxy;
using UnityEngine;

namespace Core.Systems
{
    public class SunTexture : TexturingScript
    {
        [SerializeField] private Material sunMaterial;
        [SerializeField] private List<Renderer> renderers;
        [SerializeField] private List<ParticleSystem> particles;
        public void SetMaterials(Star item, float emission)
        {
            var newMat = Instantiate(sunMaterial);
        
            newMat.color = item.GetColor();
            newMat.SetColor("_MainColor", item.GetColor());
            newMat.SetFloat("_MainEmission", emission);
            foreach (var renderer in renderers)
            {
                renderer.material = newMat;
            }
            foreach (var renderer in particles)
            {
                renderer.startColor = newMat.color;
                renderer.GetComponent<ParticleSystemRenderer>().material.color = newMat.color;
            }
        }
    
    }
}
