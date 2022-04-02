using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Location
{
    public class WorldStationQuests : MonoBehaviour
    {
        public static WorldStationQuests Instance;

        private void Awake()
        {
            SetInstance();
        }

        public static void SetInstance()
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<WorldStationQuests>();
            }
        }
        
        [System.Serializable]
        public class QuestFunction
        {
            public enum ExecuteType
            {
                Init, IsCompleteCheck, isComplete, ButtonDisplay
            }
            public int questType;
            public MonoBehaviour script;
            public List<string> initEvents;
            public List<string> isCompliteEvents;
            public List<string> OnButtonDisplay;
            public List<string> OnComplitedEvents;


            public void Execute(Quest quest, ExecuteType type)
            {
                switch (type)
                {
                    case ExecuteType.Init:
                        for (int i = 0; i < initEvents.Count; i++)
                        {
                            script.GetType().GetMethod(initEvents[i])?.Invoke(script, new object[] {quest});
                        }
                        break;
                    case ExecuteType.IsCompleteCheck:
                        for (int i = 0; i < isCompliteEvents.Count; i++)
                        {
                            script.GetType().GetMethod(isCompliteEvents[i])?.Invoke(script, new object[] {quest});
                        }
                        break;
                    case ExecuteType.isComplete:
                        for (int i = 0; i < OnComplitedEvents.Count; i++)
                        {
                            script.GetType().GetMethod(OnComplitedEvents[i])?.Invoke(script, new object[] {quest});
                        }
                        break;
                    case ExecuteType.ButtonDisplay:
                        for (int i = 0; i < OnButtonDisplay.Count; i++)
                        {
                            script.GetType().GetMethod(OnButtonDisplay[i])?.Invoke(script, new object[] {quest});
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
                
            }
        }

        public List<QuestFunction> QuestsMethods = new List<QuestFunction>();

        public QuestFunction GetEventByID(int id)
        {
            return QuestsMethods.Find(x => x.questType == id);
        }
    }

}