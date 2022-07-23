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
            var tutorial = TutorialsManager.tutorial;
            if (tutorial.CommunitsBaseStats == null)
            {
                tutorial.CommunitsBaseStats = new TutorialsManager.Tutorial.CommBaseData();
                tutorial.CommunitsBaseStats.killedDialogs = list.GetDead();
                TutorialsManager.SaveTutorial(tutorial);
                scenes.LoadScene(Scenes.OutBaseDemo);
            }
        }
    }
}
