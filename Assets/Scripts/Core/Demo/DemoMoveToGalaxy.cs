using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.Demo
{
    public class DemoMoveToGalaxy : MonoBehaviour
    {
        public static void LoadGalaxyLocation()
        {
            var manager = PlayerDataManager.Instance.Services.TutorialsManager;
            var tutor = manager.TutorialData;
            tutor.ValuesData.AddWatchDemo(Demos.Start);
            manager.Save();
            World.LoadLevelAsync(Scenes.Galaxy);
        }

        public void LoadGalaxy()
        {
            LoadGalaxyLocation();
        }
    }
}
