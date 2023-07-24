using System.Collections;
using System.Collections.Generic;
using Core.Garage;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class ExitDoor : MonoBehaviour
    {
        [SerializeField] private DialogMansList list;
        [SerializeField] private FaderMultiScenes scenes;
        public void Init()
        {
            var manager = PlayerDataManager.Instance.Services.TutorialsManager;
            var tutorial = manager.TutorialData;

            if (tutorial.MainBaseData.KilledDialogs.Count == 0)
            {
                tutorial.MainBaseData.SetKilled(list.GetDead());
                manager.SaveTutorial();
                scenes.LoadScene(Scenes.OutBaseDemo);
            }
        }
    }
}
