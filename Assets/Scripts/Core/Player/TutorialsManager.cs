using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Game;
using Core.Location;
using Core.Systems;
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
            public bool m1_Dialog1;
            public bool m1_QuestCompleted;
            public string startSystemName = "";
            public string baseSystemName = "";
            
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
