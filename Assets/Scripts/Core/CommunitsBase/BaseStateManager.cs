using System;
using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class BaseStateManager : StartupObject
    {
        [SerializeField] private GameObject doorDemo, normalDoor;
        [SerializeField] private CommunistsQuests quests;

        public override void Init(PlayerDataManager playerDataManager)
        {
            base.Init(playerDataManager);
            
            var tutor = playerDataManager.Services.TutorialsManager.TutorialData;
            if (tutor == null || tutor.MainBaseData.HaveBase == false || tutor.ValuesData.HaveWatchDemo(Demos.BaseDemo) == false)
            {
                quests.SetToBarmanQuests();
                doorDemo.SetActive(true);
                normalDoor.SetActive(false);
            }
            else
            {
                doorDemo.SetActive(false);
                normalDoor.SetActive(true);
            }
        }
    }
}
