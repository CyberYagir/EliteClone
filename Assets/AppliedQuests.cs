using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class AppliedQuests : MonoBehaviour
{
    public static AppliedQuests Instance;
    [SerializeField] private List<Quest> quests;

    private void Awake()
    {
        Instance = this;
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
