using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.ActivatorDemo
{
    public class ActivatorManager : MonoBehaviour
    {
        public void ChangeLevel()
        {
            var tutor = TutorialsManager.LoadTutorial();
            tutor.seeTranslatorDemo = true;
            TutorialsManager.SaveTutorial(tutor);
            World.LoadLevel((Scenes) PlayerPrefs.GetInt("last_level"));
        }
    }
}
