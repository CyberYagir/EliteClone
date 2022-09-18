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
                public bool isSeeDemo;
            }
            
            /// ============= M1
            public bool isDemoEnd;
            public bool m1_Dialog1;
            
            /// ============= M2
            public string startSystemName = "";
            public string baseSystemName = "";
            
            public CommBaseData CommunitsBaseStats = null;
            
            public bool m2_Dialog2;
            
            public bool seeTranslatorDemo;
            
            
            public bool m3_Dialog3;
            public bool m3_Dialog4;
            
            /// ============= M3
            
            
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
