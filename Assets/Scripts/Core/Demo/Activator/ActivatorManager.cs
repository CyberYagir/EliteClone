using System.Collections;
using System.Collections.Generic;
using Core.Core.Inject.FoldersManagerService;
using Core.Core.Inject.GlobalDataService;
using Core.PlayerScripts;
using UnityEngine;
using Zenject;

namespace Core.ActivatorDemo
{
    public class ActivatorManager : MonoBehaviour
    {
        private FolderManagerService folderManagerService;
        private SolarSystemService solarSystemService;

        [Inject]
        public void Constructor(
            FolderManagerService folderManagerService, 
            SolarSystemService solarSystemService)
        {
            this.solarSystemService = solarSystemService;
            this.folderManagerService = folderManagerService;
        }
        public void ChangeLevel()
        {
            var tutor = TutorialsManager.LoadTutorial(folderManagerService);
            tutor.seeTranslatorDemo = true;
            TutorialsManager.SaveTutorial(tutor, folderManagerService);
            World.LoadLevel((Scenes) PlayerPrefs.GetInt("last_level"));
        }
    }
}
