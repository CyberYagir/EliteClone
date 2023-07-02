using System.Collections.Generic;
using Core.Location;
using Core.Quests;
using Core.Systems;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class AppliedQuests : Singleton<AppliedQuests>
    {
        
        public class QuestData
        {
            public int seed;
            public Character character;
            public string stationName, solarName;
            public Quest.QuestCompleted state;
        }
        
        public List<Quest> quests { get; private set; } = new List<Quest>();
        public Event OnChangeQuests = new Event();
        public Event OnNotify = new Event();

        [SerializeField] private QuestsMethodObject methods;

        private WorldDataHandler worldDataHandler;
        
    
        private void Awake()
        {
            Single(this);
            worldDataHandler = PlayerDataManager.Instance.WorldHandler;
        }

        private void Start()
        {
            OnChangeQuests.Run();
        }

        public List<QuestData> GetData()
        {
            List<QuestData> data = new List<QuestData>();
            for (int i = 0; i < quests.Count; i++)
            {
                data.Add(new QuestData
                {
                    seed = quests[i].questID, 
                    character = quests[i].quester, 
                    stationName = quests[i].appliedStation, 
                    solarName = quests[i].appliedSolar,
                    state = quests[i].questState
                });
            }
            return data;
        }
        public void LoadList(List<QuestData> qts)
        {
            for (int i = 0; i < qts.Count; i++)
            {
                var qust = new Quest(qts[i].seed, qts[i].character, qts[i].stationName, qts[i].solarName, methods);
                qust.questState = qts[i].state;
                quests.Add(qust);
            }
        }

        public bool FinishQuest(int questID)
        {
            var quest = quests.Find(x => x.questID == questID);
            if (quest != null)
            {
                quest.questState = Quest.QuestCompleted.Rewarded; 
                ReputationManager.Instance.AddRating(quest.quester.fraction, quest.questCost);
                quest.quester.Reset();
                return true;
            }

            OnChangeQuests.Run();
            return false;
        }
    
        public void ApplyQuest(Quest quest, bool triggerNotify = true)
        {
            if (quest.toTransfer.Count != 0 && !quest.keyValues.ContainsKey("NoAddTransfer") && worldDataHandler.ShipPlayer.cargo.AddItems(quest.toTransfer))
            {
                quests.Add(quest);
            }
            else
            {
                quests.Add(quest);
            }

            if (triggerNotify)
            {
                OnNotify.Run();
            }
            OnChangeQuests.Run();
        }

        public void CancelQuest(Quest quest)
        {
            if (quest == null) return;
            if (quest.toTransfer.Count != 0 && !quest.keyValues.ContainsKey("NoAddTransfer") && worldDataHandler.ShipPlayer.cargo.RemoveItems(quest.toTransfer))
            {
                quests.RemoveAll(x => x.questID == quest.questID);
            }
            else
            {
                quests.RemoveAll(x => x.questID == quest.questID);
            }
            OnChangeQuests.Run();
        }

        public bool IsQuestApplied(int id)
        {
            return quests.Find(x => x.questID == id) != null;
        }
    }
}
