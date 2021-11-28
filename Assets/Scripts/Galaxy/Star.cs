using UnityEngine;

[System.Serializable]
public class Star: SpaceObject
{
    public enum StarType { M, K, G, F, A, B, O}
    public StarType starType;

    public Star(StarType starType, System.Random rnd)
    {
        name = GenerateName(rnd);
        this.starType = starType;
        switch (starType)
        {
            case StarType.M:
                mass = GalaxyGenerator.NextDecimal(rnd, 0.30m, 0.7m);
                radius = GalaxyGenerator.NextDecimal(rnd, 0.3m, 0.8m);
                break;
            case StarType.K:
                mass = GalaxyGenerator.NextDecimal(rnd, 0.8m, 1.3m);
                radius = GalaxyGenerator.NextDecimal(rnd, 0.9m, 1.1m);
                break;
            case StarType.G:
                mass = GalaxyGenerator.NextDecimal(rnd, 1.6m, 2.6m);
                radius = GalaxyGenerator.NextDecimal(rnd, 1.1m, 1.3m);
                break;
            case StarType.F:
                mass = GalaxyGenerator.NextDecimal(rnd, 3m, 16m);
                radius = GalaxyGenerator.NextDecimal(rnd, 1.3m, 2.5m);
                break;
            case StarType.A:
                mass = GalaxyGenerator.NextDecimal(rnd, 18m, 55m);
                radius = GalaxyGenerator.NextDecimal(rnd, 2.5m, 7m);
                break;
            case StarType.B:
                mass = GalaxyGenerator.NextDecimal(rnd, 18m, 40m);
                radius = GalaxyGenerator.NextDecimal(rnd, 7m, 15m);
                break;
            case StarType.O:
                mass = GalaxyGenerator.NextDecimal(rnd, 55m, 120m);
                radius = GalaxyGenerator.NextDecimal(rnd, 15m, 40m);
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
            case Star.StarType.M:
                return Color.red;
            case Star.StarType.K:
                return new Color32(245, 140, 4, 255);
            case Star.StarType.G:
                return Color.yellow;
            case Star.StarType.F:
                return new Color32(249, 255, 155, 255);
                break;
            case Star.StarType.A:
                return Color.white;
                break;
            case Star.StarType.B:
                return new Color32(110, 235, 255, 255);
            case Star.StarType.O:
                return new Color32(0, 182, 255, 255);
            default:
                return Color.white;
        }
    }

    public Star()
    {

    }

    public static string GenerateName(System.Random rnd)
    {
        if (GalaxyGenerator.words == null)
        {
            GalaxyGenerator.GetWords();
        }
        var nameID = rnd.Next(0, GalaxyGenerator.words.Length);
        var str = GalaxyGenerator.words[nameID];

        //Debug.Log(str);
        if (str.Length == 1)
            str = char.ToUpper(str[0]).ToString();
        else
            str = (char.ToUpper(str[0]) + str.Substring(1));

        return str + " " + rnd.Next(0, 999999).ToString("0000000");
    }
}
