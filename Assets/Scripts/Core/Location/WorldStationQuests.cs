using System;
using System.Collections.Generic;
using Core.Quests;
using UnityEngine;

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
        
        [Serializable]
        public class QuestFunction
        {
            public enum ExecuteType
            {
                Init, IsCompleteCheck, isCompleted, ButtonDisplay
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
                    case ExecuteType.isCompleted:
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

        [SerializeField] private QuestsMethodObject QuestsMethods;

        public QuestFunction GetEventByID(int id)
        {
            return QuestsMethods.Methods.Find(x => x.questType == id);
        }
    }

}