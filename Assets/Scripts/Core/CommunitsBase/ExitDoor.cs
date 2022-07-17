using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class ExitDoor : MonoBehaviour
    {
        [SerializeField] private DialogMansList list;
        public void Init()
        {
            var tutorial = TutorialsManager.tutorial;
            if (tutorial.CommunitsBaseStats == null)
            {
                tutorial.CommunitsBaseStats = new TutorialsManager.Tutorial.CommBaseData();
                tutorial.CommunitsBaseStats.killedDialogs = list.GetDead();
                tutorial.CommunitsBaseStats.completeBarmanQuest = true;
                TutorialsManager.SaveTutorial(tutorial);
            }
            
            
        }
    }
}
