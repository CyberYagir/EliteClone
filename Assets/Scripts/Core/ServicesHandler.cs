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

        public TutorialsManager TutorialsManager => tutorialsManager;

        public void Init()
        {
            inputService.Init();
            itemsManager.Init();
            cursorManager.Init();
            tutorialsManager.LoadTutorial();

        }
    }
}