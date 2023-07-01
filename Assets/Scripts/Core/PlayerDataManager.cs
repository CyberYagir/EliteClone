using System.IO;
using Core.Core.Inject.FoldersManagerService;
using Core.Galaxy;
using Core.Garage;
using Core.Init;
using Core.Location;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerDataManager : MonoBehaviour
    {
        public static float GenerateProgress;
        private bool loading;
        
        
        private FolderManagerService foldersService;

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

        [Inject]
        public void Constructor(FolderManagerService folderManagerService)
        {
            foldersService = folderManagerService;
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
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene()
        {
            var haveGalaxy = GalaxyGenerator.LoadSystems();
            if (File.Exists(foldersService.CurrentLocationFile) && haveGalaxy)
            {
                World.LoadLevel(Scenes.Location);
            }
            else if (File.Exists(foldersService.CurrentSystemFile) && haveGalaxy)
            {
                World.LoadLevel(Scenes.System);
            }
            else
            {
                if (File.Exists(foldersService.GalaxyFile))
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

        public static void SaveAll()
        {
            if (GarageDataCollect.Instance != null)
            {
                FindObjectOfType<GarageExitButton>().SaveIfCan();
            }

            if (Player.inst != null)
            {
                Player.inst.saves.Save();
                if (World.Scene == Scenes.System)
                {
                    SolarSystemGenerator.SaveSystem();
                }
            }
        }
    }
}
