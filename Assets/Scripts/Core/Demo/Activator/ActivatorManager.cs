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
            var tutorManager = PlayerDataManager.Instance.Services.TutorialsManager;
            var tutor = tutorManager.TutorialData;
                
            tutor.seeTranslatorDemo = true;
            tutorManager.SaveTutorial();
            World.LoadLevel((Scenes) PlayerPrefs.GetInt("last_level"));
        }
    }
}
