using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTexture : TexturingScript
{
    [SerializeField] private Material sunMaterial;
    [SerializeField] private List<Renderer> renderers;

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
    }
    
}
