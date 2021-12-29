using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class AppliedQuests : MonoBehaviour
{
    public static AppliedQuests Instance;
    
    public class QuestData
    {
        public int seed;
        public Character character;
        public string stationName, solarName;
    }
    
    public List<Quest> quests { get; private set; } = new List<Quest>();

    
    
    private void Awake()
    {
        Instance = this;
    }


    public List<QuestData> GetData()
    {
        List<QuestData> data = new List<QuestData>();
        for (int i = 0; i < quests.Count; i++)
        {
            data.Add(new QuestData(){seed = quests[i].questID, character = quests[i].quester, stationName = quests[i].appliedStation, solarName = quests[i].appliedSolar});
        }
        return data;
    }
    public void LoadList(List<QuestData> qts)
    {
        for (int i = 0; i < qts.Count; i++)
        {
            var qust = new Quest(qts[i].seed, qts[i].character, qts[i].stationName, qts[i].solarName);
            quests.Add(qust);
        }
    }
    public void ApplyQuest(Quest quest)
    {
        quests.Add(quest);
    }
    public void CancleQuest(Quest quest)
    {
        quests.RemoveAll(x=>x.questID == quest.questID);
    }

    public bool IsQuestApplied(int id)
    {
        return quests.Find(x => x.questID == id) != null;
    }
}
