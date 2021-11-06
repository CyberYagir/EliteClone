using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyPoint : MonoBehaviour
{
    public SolarSystem solarSystem;
    public ParticleSystem particles;
    [SerializeField] ParticleSystemRenderer particlesRenderer;
    [SerializeField] Texture2D[] textures;
    private void Start()
    {
        switch (solarSystem.stars[0].starType)
        {
            case Star.StarType.M:
                particles.startColor = Color.red;
                break;
            case Star.StarType.K:
                particles.startColor = new Color32(245, 140, 4, 255);
                break;
            case Star.StarType.G:
                particles.startColor = Color.yellow;
                break;
            case Star.StarType.F:
                particles.startColor = new Color32(249, 255, 155, 255);
                break;
            case Star.StarType.A:
                particles.startColor = Color.white;
                break;
            case Star.StarType.B:
                particles.startColor = new Color32(110, 235, 255, 255);
                break;
            case Star.StarType.O:
                particles.startColor = Color.blue;
                break;
            default:
                break;
        }
        particlesRenderer.material.mainTexture = textures[Random.Range(0, textures.Length)];
    }


    private void OnMouseDown()
    {
        GalaxyManager.selectedPoint = this;
        if (LineDrawer.instance.stars.Contains(this))
        {
            LineDrawer.instance.stars.Remove(this);
        }

        LineDrawer.instance.stars.Insert(0, this);
        LineDrawer.instance.UpdateLines();
    }
}
