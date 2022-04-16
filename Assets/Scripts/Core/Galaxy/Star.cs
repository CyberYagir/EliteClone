using System;
using UnityEngine;
using Random = System.Random;

namespace Core.Galaxy
{
    [Serializable]
    public class Star: SpaceObject
    {
        public enum StarType { M, K, G, F, A, B, O, Hole}
        public StarType starType;

        public Star(StarType starType, Random rnd)
        {
            name = GenerateName(rnd);
            this.starType = starType;
            switch (starType)
            {
                case StarType.M:
                    mass = GalaxyGenerator.NextDecimal(rnd, 0.30f, 0.7f);
                    radius = GalaxyGenerator.NextDecimal(rnd, 0.8f, 1f);
                    break;
                case StarType.K:
                    mass = GalaxyGenerator.NextDecimal(rnd, 1f, 1.3f);
                    radius = GalaxyGenerator.NextDecimal(rnd, 0.9f, 1.1f);
                    break;
                case StarType.G:
                    mass = GalaxyGenerator.NextDecimal(rnd, 1.6f, 2.6f);
                    radius = GalaxyGenerator.NextDecimal(rnd, 1.1f, 1.3f);
                    break;
                case StarType.F:
                    mass = GalaxyGenerator.NextDecimal(rnd, 3f, 16f);
                    radius = GalaxyGenerator.NextDecimal(rnd, 1.3f, 2.5f);
                    break;
                case StarType.A:
                    mass = GalaxyGenerator.NextDecimal(rnd, 18f, 55f);
                    radius = GalaxyGenerator.NextDecimal(rnd, 2.5f, 7f);
                    break;
                case StarType.B:
                    mass = GalaxyGenerator.NextDecimal(rnd, 18f, 40f);
                    radius = GalaxyGenerator.NextDecimal(rnd, 7f, 15f);
                    break;
                case StarType.O:
                    mass = GalaxyGenerator.NextDecimal(rnd, 55f, 120f);
                    radius = GalaxyGenerator.NextDecimal(rnd, 15f, 40f);
                    break;
                case StarType.Hole:
                    mass = GalaxyGenerator.NextDecimal(rnd, 100f, 220f);
                    radius = GalaxyGenerator.NextDecimal(rnd, 30f, 70f);
                    break;
                default:
                    mass = -1;
                    radius = -1;
                    break;
            }
        }

        public Color GetColor()
        {
            switch (starType)
            {
                case StarType.M:
                    return Color.red;
                case StarType.K:
                    return new Color32(245, 140, 4, 255);
                case StarType.G:
                    return Color.yellow;
                case StarType.F:
                    return new Color32(249, 255, 155, 255);
                    break;
                case StarType.A:
                    return Color.white;
                    break;
                case StarType.B:
                    return new Color32(110, 235, 255, 255);
                case StarType.O:
                    return new Color32(0, 182, 255, 255);
                default:
                    return Color.white;
            }
        }

        public Star()
        {

        }

        public static string GenerateName(Random rnd)
        {
            if (GalaxyGenerator.words == null)
            {
                GalaxyGenerator.GetWords();
            }
            var nameID = rnd.Next(0, GalaxyGenerator.words.Length);
            var str = GalaxyGenerator.words[nameID];

            if (str.Length == 1)
                str = char.ToUpper(str[0]).ToString();
            else
                str = (char.ToUpper(str[0]) + str.Substring(1));

            return str + " " + rnd.Next(0, 999999).ToString("0000000");
        }
    }
}
