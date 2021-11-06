using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        GalaxyManager.Select(this);
        if (LineDrawer.instance.stars.Contains(this))
        {
            LineDrawer.instance.stars.Remove(this);
        }

        LineDrawer.instance.stars.Insert(0, this);
        LineDrawer.instance.UpdateLines();
    }
}
