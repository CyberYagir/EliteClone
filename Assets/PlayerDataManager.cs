using System;
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
    public static string galaxyFile, currentSystemFile, currentLocationFile, playerDataFile;
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
                    DelChilds();
                }
            }
        }
    }

    public void DelChilds()
    {
        foreach (Transform it in transform)
        {
            Destroy(it.gameObject);
        }
    }
    
    private void Awake()
    {
        if (Application.loadedLevelName != "Menu")
        {
            InitDataManager();
        }
    }

    public void InitDataManager()
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

            FoldersManage();
        }
    }


    public void FoldersManage()
    {
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
        currentLocationFile = globalFolder + "location.json";
        playerDataFile = globalFolder + "player.json";
    }

    public void LoadScene()
    {
        if (File.Exists(currentLocationFile))
        {
            Application.LoadLevel("Location");
            DelChilds();
        }
        else if (File.Exists(currentSystemFile))
        {
            Application.LoadLevel("System");
            DelChilds();
        }
        else
        {
            if (File.Exists(galaxyFile))
            {
                loading = true;
                generateProgress = 1;
                DelChilds();
            }
            else
            {
                StartCoroutine(GalaxyGenerator.GenerateGalaxy(galaxySeed));
                loading = true;
            }
        }
    }
}
