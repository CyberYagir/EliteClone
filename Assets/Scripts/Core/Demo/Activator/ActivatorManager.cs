using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;

namespace Core
{
    public enum Demos
    {
        Start,
        BaseDemo,
        Translator
    }
}

namespace Core.ActivatorDemo
{
    public class ActivatorManager : MonoBehaviour
    {
        public void ChangeLevel()
        {
            var tutorManager = PlayerDataManager.Instance.Services.TutorialsManager;
            var tutor = tutorManager.TutorialData;
                
            tutor.ValuesData.AddWatchDemo(Demos.Translator);
            
            
            
            tutorManager.SaveTutorial();
            World.LoadLevel((Scenes) PlayerPrefs.GetInt("last_level"));
        }
    }
}
