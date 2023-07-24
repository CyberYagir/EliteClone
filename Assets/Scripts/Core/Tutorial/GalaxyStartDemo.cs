using System;
using System.Collections;
using System.Collections.Generic;
using Core.Galaxy;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.Tutorial
{
    public class GalaxyStartDemo : MonoBehaviour
    {
        private void Awake()
        {
            var tutor = PlayerDataManager.Instance.Services.TutorialsManager.TutorialData;
            if (!tutor.ValuesData.HaveWatchDemo(Demos.Start))
            {
                World.LoadLevel(Scenes.Demo);
                FindObjectOfType<GalaxyGenerator>().enabled = false;
            }
        }
    }
}
