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
        public static Tutorial tutorial;
        
        [System.Serializable]
        public class Tutorial
        {
            public class CommBaseData
            {
                public List<string> killedDialogs;
                public bool completeBarmanQuest;
                public bool isSeeDemo;
            }
            
            
            public bool isDemoEnd;
            public bool m1_Dialog1;
            public CommBaseData CommunitsBaseStats = null;
            
            
            public string startSystemName = "";
            public string baseSystemName = "";
            
            public Tutorial()
            {
            }

        }




        public static Tutorial LoadTutorial()
        {
            if (File.Exists(PlayerDataManager.TutorialsFile) && tutorial == null)
            {
                tutorial = JsonConvert.DeserializeObject<Tutorial>(File.ReadAllText(PlayerDataManager.TutorialsFile));
            }

            if (!File.Exists(PlayerDataManager.TutorialsFile))
            {
                tutorial = new Tutorial();
                SaveTutorial(tutorial);
            }

            return tutorial;
        }

        public static void SaveTutorial(Tutorial tutor)
        {
            tutorial = tutor;
            File.WriteAllText(PlayerDataManager.TutorialsFile, JsonConvert.SerializeObject(tutor));
        }
    }
}
