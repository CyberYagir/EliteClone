using System.Text;
using UnityEngine;

public class WorldOrbitalStation : MonoBehaviour
{
  
    public static string[] firstNames, lastNames;
    [SerializeField] private int uniqSeed;
    public Transform spawnPoint;


    public static void InitNames()
    {
        if (firstNames == null)
        {
            firstNames = LoadFromFile("names");
            lastNames = LoadFromFile("lastnames");
        }
    }

    private static string[] LoadFromFile(string nm)
    {
        TextAsset mytxtData = (TextAsset) Resources.Load(nm);
        var wrds = mytxtData.text;
        return wrds.Split('/');
    }


    private void Start()
    {
        InitNames();
        uniqSeed = 0;
        foreach (var ch in Encoding.ASCII.GetBytes(transform.name))
        {
            uniqSeed += ch;
        }
        uniqSeed *= uniqSeed;
    }

}
