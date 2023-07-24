using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.PlayerScripts
{
    [System.Serializable]
    public class TutorialSO : ScriptableObject
    {
        [System.Serializable]
        public class ComunistsMainBase
        {
            [SerializeField] private bool haveBase = false;
            [SerializeField] private string startSystemName = String.Empty;
            [SerializeField] private string baseSystemName = String.Empty;

            [SerializeField] private List<string> killedDialogs;
            
            public string BaseSystemName => baseSystemName;
            public string StartSystemName => startSystemName;
            public bool HaveBase => haveBase;
            public List<string> KilledDialogs => killedDialogs;


            public void SetFistSystem(string startSystemName)
            {
                this.startSystemName = startSystemName;
            }
            
            public void SetBase(string baseSystemName)
            {
                this.baseSystemName = baseSystemName;
                haveBase = true;
            }

            public void SetKilled(List<string> getDead)
            {
                killedDialogs = getDead;
            }
        }

        [System.Serializable]
        public class PlayerTutorialQuests
        {
            [SerializeField] private int questID = 0;

            public int QuestID => questID;

            public void NextTutorialQuest()
            {
                questID = QuestID + 1;
            }
        }
        [System.Serializable]
        
        public class PlayerValuesData
        {
            [SerializeField] private List<string> tags = new List<string>();


            public void AddValue(string value)
            {
                value = value.ToLower().Trim();
                if (!HaveValue(value))
                {
                    tags.Add(value);
                }
            }

            public bool HaveValue(string value)
            {
                return tags.Contains(value.ToLower().Trim());
            }

            public void AddWatchDemo(Demos value)
            {
                AddValue("Watch_Demo_" + value);
            }
            public bool HaveWatchDemo(Demos value)
            {
                return HaveValue("Watch_Demo_" + value);
            }
        }
        
        [SerializeField] private ComunistsMainBase mainBaseData;
        [SerializeField] private PlayerTutorialQuests tutorialQuestsData;
        [SerializeField] private PlayerValuesData valuesData;

        public PlayerValuesData ValuesData => valuesData;

        public PlayerTutorialQuests TutorialQuestsData => tutorialQuestsData;

        public ComunistsMainBase MainBaseData => mainBaseData;
    }
}