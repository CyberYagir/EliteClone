using Core.Init;
using Core.PlayerScripts;
using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class ServicesHandler
    {
        [SerializeField] private InputService inputService;
        [SerializeField] private ItemsManager itemsManager;
        [SerializeField] private CursorManager cursorManager;
        [SerializeField] private TutorialsManager tutorialsManager;
        [SerializeField] private WorldStructureService worldStructuresManager;

        public TutorialsManager TutorialsManager => tutorialsManager;
        public WorldStructureService WorldStructuresManager => worldStructuresManager;

        public void Init(FilesSystemHandler filesSystemHandler)
        {
            inputService.Init();
            itemsManager.Init();
            cursorManager.Init();
            tutorialsManager.Init(filesSystemHandler.TutorialsFile);
            worldStructuresManager.Init(filesSystemHandler.StructuresFile);
        }
    }
}