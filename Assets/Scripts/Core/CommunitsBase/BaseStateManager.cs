using System;
using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class BaseStateManager : MonoBehaviour
    {
        [SerializeField] private GameObject doorDemo, normalDoor;
        [SerializeField] private CommunistsQuests quests;

        private void Start()
        {
            var tutor = PlayerDataManager.Instance.Services.TutorialsManager.TutorialData;
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
