using System;
using System.Collections;
using System.Collections.Generic;
using Core.Galaxy;
using Core.PlayerScripts;
using UnityEngine;
using Zenject;

namespace Core.Tutorial
{
    public class GalaxyStartDemo : MonoBehaviour
    {
        [Inject]
        private void Constructor()
        {
            var tutor = TutorialsManager.LoadTutorial();
            if (!tutor.isDemoEnd)
            {
                World.LoadLevel(Scenes.Demo);
                FindObjectOfType<GalaxyGenerator>().enabled = false;
            }
        }
    }
}
