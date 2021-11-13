using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;
    public static SolarSystem currentSolarSystem;
    public static int galaxySeed = -1;
    public static string playerFolder, globalFolder, cacheSystemsFolder, root;
    public static string galaxyFile, currentSystemFile;
    public static float generateProgress;
    bool loading = false;
    public static bool saveSystems = true;
    private void Update()
    {
        if (Application.loadedLevelName == "Init")
        {
            if (loading)
            {
                if (generateProgress == 1)
                {
                    Application.LoadLevelAsync("Galaxy");
                    enabled = false;
                }
            }
        }
    }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;

        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (!Directory.Exists(Directory.GetParent(Application.dataPath).FullName + "/Saves"))
            {
                Directory.CreateDirectory(Directory.GetParent(Application.dataPath).FullName + "/Saves");
            }
            root = Directory.GetParent(Application.dataPath).FullName + "/Saves";
            if (!Directory.Exists(root + "/Player"))
            {
                Directory.CreateDirectory(root + "/Player/");
            }
            playerFolder = root + "/Player/";
            if (!Directory.Exists(root + "/Player/Global"))
            {
                Directory.CreateDirectory(root + "/Player/Global");
            }
            globalFolder = root + "/Player/Global/";
            if (!Directory.Exists(root + "/Player/Locations"))
            {
                Directory.CreateDirectory(root + "/Player/Locations");
            }
            cacheSystemsFolder = root + "/Player/Locations/";

            galaxyFile = globalFolder + "galaxy.json";
            currentSystemFile = globalFolder + "system.json";

            if (File.Exists(currentSystemFile))
            {
                Application.LoadLevelAsync("System");
            }
            else
            {
                if (File.Exists(galaxyFile))
                {
                    loading = true;
                    generateProgress = 1;
                }
                else
                {
                    StartCoroutine(GalaxyGenerator.GenerateGalaxy(galaxySeed));
                    loading = true;
                }
            }
        }
    }
}
