using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class TutorialsManager : MonoBehaviour
    {
        [System.Serializable]
        public class Tutorial
        {
            public bool isDemoEnd;
            public bool isShipControlComplete;
            public bool isUITutorialComplete;
            public bool isLandingStationComplete;
            public bool isGarageTutorialComplete;
            public bool isMineTutorialComplete;

            public Tutorial()
            {
            }
        }



        public static Tutorial LoadTutorial()
        {
            if (File.Exists(PlayerDataManager.TutorialsFile))
            {
                return JsonConvert.DeserializeObject<Tutorial>(File.ReadAllText(PlayerDataManager.TutorialsFile));
            }
            else
            {
                return new Tutorial();
            }
        }

        public static void SaveTutorial(Tutorial tutor)
        {
            File.WriteAllText(PlayerDataManager.TutorialsFile, JsonConvert.SerializeObject(tutor));
        }
    }
}
