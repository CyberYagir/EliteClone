﻿using System.Collections.Generic;
using UnityEngine;

namespace Core.PlayerScripts
{
    [System.Serializable]
    public class TutorialSO : ScriptableObject
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
        

    }
}