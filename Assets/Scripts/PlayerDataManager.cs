using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public static SolarSystem CurrentSolarSystem;
    public static int galaxySeed = -1;
    public static string PlayerFolder, GlobalFolder, CacheSystemsFolder, RootFolder;
    public static string GalaxyFile, CurrentSystemFile, CurrentLocationFile, PlayerDataFile;
    public static float GenerateProgress;
    private bool loading = false;
    private void Update()
    {
        if (World.Scene == Scenes.Init)
        {
            if (loading)
            {
                if (GenerateProgress == 1)
                {
                    World.LoadLevel(Scenes.Galaxy);
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
        InitDataManager();
    }

    private void Start()
    {
        if (World.Scene != Scenes.Init)
        {
            LoadScene();
        }
    }

    public void InitDataManager()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;

        }
        else
        {
            Instance = this;
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

        RootFolder = Directory.GetParent(Application.dataPath)?.FullName + "/Saves";
        
        if (!Directory.Exists(RootFolder + "/Player"))
        {
            Directory.CreateDirectory(RootFolder + "/Player/");
        }

        PlayerFolder = RootFolder + "/Player/";
        if (!Directory.Exists(PlayerFolder + "/Global"))
        {
            Directory.CreateDirectory(PlayerFolder + "/Global");
        }

        GlobalFolder = PlayerFolder + "/Global/";
        if (!Directory.Exists(PlayerFolder + "/Locations"))
        {
            Directory.CreateDirectory(PlayerFolder + "/Locations");
        }

        CacheSystemsFolder = PlayerFolder + "/Locations/";

        GalaxyFile = GlobalFolder + "galaxy.json";
        CurrentSystemFile = GlobalFolder + "system.json";
        CurrentLocationFile = GlobalFolder + "location.json";
        PlayerDataFile = GlobalFolder + "player.json";
    }

    public void LoadScene()
    {
        if (File.Exists(CurrentLocationFile))
        {
            World.LoadLevel(Scenes.Location);
            DelChilds();
        }
        else if (File.Exists(CurrentSystemFile))
        {
            World.LoadLevel(Scenes.System);
            DelChilds();
        }
        else
        {
            if (File.Exists(GalaxyFile))
            {
                loading = true;
                GenerateProgress = 1;
                DelChilds();
            }
            else
            {
                if (!loading)
                {
                    StartCoroutine(GalaxyGenerator.GenerateGalaxy(galaxySeed));
                    loading = true;
                }
            }
        }
    }
}
