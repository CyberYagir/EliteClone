using System.IO;
using Core.Galaxy;
using Core.Garage;
using Core.Init;
using Core.Location;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class RunHandler
    {
        [SerializeField] private bool isPaused;

        public bool IsPaused => isPaused;


        public void SetPause(bool state)
        {
            isPaused = state;
        }
    }
    
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        public static InitOptions.PlayerConfig PlayerConfig;
        public static float GenerateProgress;

        [SerializeField] private RunHandler runHandler;
        [SerializeField] private ServicesHandler servicesHandler;
        [SerializeField] private FilesSystemHandler filesSystemHandler;
        [SerializeField] private WorldDataHandler worldHandler;



        
        
        private bool loading;

        public ServicesHandler Services => servicesHandler;
        public FilesSystemHandler FSHandler => filesSystemHandler;

        public WorldDataHandler WorldHandler => worldHandler;

        public RunHandler RunHandler => runHandler;


        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Single(this);

            LoadStatic();
            InitDataManager();
            servicesHandler.Init();

            SolarStaticBuilder.InitStaticBuilder(worldHandler, FSHandler);
            GalaxyManager.InitStaticBuilder(worldHandler);
        }

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


        public void LoadStatic()
        {
            GalaxyGenerator.GetWords();
            OrbitalStationStaticBuilder.InitNames();
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
            filesSystemHandler.CreateFolders();
        }


        public void LoadScene()
        {
            servicesHandler.TutorialsManager.Clear();
            servicesHandler.TutorialsManager.LoadTutorial();
            
            var haveGalaxy = GalaxyGenerator.LoadSystems();
            if (File.Exists(filesSystemHandler.CurrentLocationFile) && haveGalaxy)
            {
                World.LoadLevel(Scenes.Location);
            }
            else if (File.Exists(filesSystemHandler.CurrentSystemFile) && haveGalaxy)
            {
                World.LoadLevel(Scenes.System);
            }
            else
            {
                if (File.Exists(filesSystemHandler.GalaxyFile))
                {
                    loading = true;
                    GenerateProgress = 1;
                }
                else
                {
                    if (!loading)
                    {
                        StartCoroutine(GalaxyGenerator.GenerateGalaxy(WorldHandler.GalaxySeed));
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


            var player = Instance.WorldHandler.ShipPlayer;
            
            if (player != null)
            {
                player.saves.Save();
                if (World.Scene == Scenes.System)
                {
                    SolarStaticBuilder.SaveSystem();
                }
            }
        }
    }
}
