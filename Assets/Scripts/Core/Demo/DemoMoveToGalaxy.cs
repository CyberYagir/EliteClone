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
            var tutor = TutorialsManager.LoadTutorial();
            tutor.isDemoEnd = true;
            TutorialsManager.SaveTutorial(tutor);
            World.LoadLevelAsync(Scenes.Galaxy);
        }

        public void LoadGalaxy()
        {
            LoadGalaxyLocation();
        }
    }
}
