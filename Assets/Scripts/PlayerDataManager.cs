using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    public static SolarSystem CurrentSolarSystem;

    public static InitOptions.PlayerConfig PlayerConfig;

    public static int galaxySeed = -1;
    public static string PlayerFolder, GlobalFolder, CacheSystemsFolder, RootFolder;
    public static string GalaxyFile, CurrentSystemFile, CurrentLocationFile, PlayerDataFile, ConfigFile, DeadsNPCFile;
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
                }
            }
        }
    }

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        LoadStatic();
        InitDataManager();
    }

    public void LoadStatic()
    {
        GalaxyGenerator.GetWords();
        WorldOrbitalStation.InitNames();
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


    public static void FoldersManage()
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
        DeadsNPCFile = GlobalFolder + "npcs.json";
        PlayerDataFile = GlobalFolder + "player.json";
        ConfigFile = PlayerFolder + "options.config";
    }

    public void LoadScene()
    {
        var haveGalaxy = GalaxyGenerator.LoadSystems();
        if (File.Exists(CurrentLocationFile) && haveGalaxy)
        {
            World.LoadLevel(Scenes.Location);
        }
        else if (File.Exists(CurrentSystemFile) && haveGalaxy)
        {
            World.LoadLevel(Scenes.System);
        }
        else
        {
            if (File.Exists(GalaxyFile))
            {
                loading = true;
                GenerateProgress = 1;
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
