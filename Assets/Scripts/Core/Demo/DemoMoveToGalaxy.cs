using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.Demo
{
    public class DemoMoveToGalaxy : MonoBehaviour
    {
        public void LoadGalaxy()
        {
            
            var tutor = TutorialsManager.LoadTutorial();
            tutor.isDemoEnd = true;
            TutorialsManager.SaveTutorial(tutor);
            World.LoadLevelAsync(Scenes.Galaxy);
        }
    }
}
