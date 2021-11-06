using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SolarSystemGenerator : MonoBehaviour
{
    public GameObject sunPrefab;
    private void Start()
    {
        if (PlayerDataManager.currentSolarSystem != null)
        {
            Generate();
            DrawAll();
        }
    }

    public const float scale = 5;

    public void Generate()
    {
        var pos = PlayerDataManager.currentSolarSystem.position;
        var rnd = new System.Random((int)(pos.x + pos.y + pos.z));
        var system = PlayerDataManager.currentSolarSystem;

        system.stars = new List<Star>();
        var starsCount = rnd.Next(1, 3);

        var starTypes = System.Enum.GetNames(typeof(Star.StarType)).Length;

        for (int i = 0; i < starsCount; i++)
        {
            Star.StarType type = Star.StarType.M;
            DVector spos = new DVector();

            if (i == 0)
            {
                float xCoord = (float)pos.x / (float)GalaxyGenerator.maxRadius * 20f;
                float yCoord = (float)pos.y / (float)GalaxyGenerator.maxRadius * 20f;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                type = (Star.StarType)System.Math.Round(starTypes * sample);

            }
            else
            {
                type = (Star.StarType)rnd.Next(1, starTypes);
                spos = system.stars[i - 1].position + new DVector(0, 0, system.stars[i - 1].radius) + new DVector(0, 0, rnd.Next(2, 5));
            }
            var star = new Star(type, rnd);
            star.position = spos;
            star.name = system.name.Split(' ')[0] + " " + (i != 0 ? i.ToString() : "");
            system.stars.Add(star);
        }

        var planetsCount = rnd.Next(1, 5 * starsCount);

        for (int i = 0; i < planetsCount; i++)
        {

        }

    }
    public void DrawAll()
    {

        Vector3 center = new Vector3(0,0,0);
        foreach (var star in PlayerDataManager.currentSolarSystem.stars)
        {
            center += star.position.toVector() * scale;
        }
        center /= PlayerDataManager.currentSolarSystem.stars.Count;

        int id = 1;
        foreach (var item in PlayerDataManager.currentSolarSystem.stars)
        {
            var sun = Instantiate(sunPrefab);
            sun.transform.name = item.name;
            sun.transform.position = item.position.toVector() * scale * id;
            sun.transform.localScale *= (float)item.radius * scale;

            sun.GetComponent<Renderer>().material.color = item.GetColor();
            sun.GetComponent<Renderer>().material.SetColor("_EmissionColor", item.GetColor());
            sun.GetComponent<RotateAround>().point = center;

            id++;
        }
    }
}
