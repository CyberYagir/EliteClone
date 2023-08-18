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
        [SerializeField] private SaveLoadData saveLoadData;
        public void ChangeLevel()
        {
            var tutorManager = PlayerDataManager.Instance.Services.TutorialsManager;
            var tutor = tutorManager.TutorialData;
                
            tutor.ValuesData.AddWatchDemo(Demos.Translator);
            tutor.TutorialQuestsData.NextTutorialQuest();
            tutorManager.Save();
            
            var data = saveLoadData.LoadData();

            data.Keys.Remove("loc_start_on_pit");

            saveLoadData.SaveData(data);
            
            World.LoadLevel((Scenes) PlayerPrefs.GetInt("last_level"));
        }
    }
}
