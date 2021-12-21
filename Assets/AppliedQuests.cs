using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class AppliedQuests : MonoBehaviour
{
    public static AppliedQuests Instance;
    public List<Quest> quests { get; private set; } = new List<Quest>();

    private void Awake()
    {
        Instance = this;
    }

    public void LoadList(List<Quest> qts)
    {
        quests = qts;
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
