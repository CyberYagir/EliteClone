using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UnityEngine;

public class QuestInfoUI : BaseTab
{
    [SerializeField] private List<ButtonEffect> items;
    [SerializeField] private BaseTab questList;
    [Space] [SerializeField] private TMP_Text targetName;
    [SerializeField] private TMP_Text targetSystem;
    [SerializeField] private TMP_Text rewardText, jumpsCount;
    [SerializeField] private TMP_Text buttonText;

    private Quest currentQuest;
    
    private void Start()
    {
        (questList as QuestListUI).OnChangeSelected += UpdateData;
        upDownUI.OnChangeSelected += SelectButton;
        Player.OnSceneChanged += () =>
        {
            currentQuest = null;
            Clear();
            Disable();
        };
    }

    private void SelectButton()
    {
        if (upDownUI.selectedIndex == 0)
        {
            if (!AppliedQuests.Instance.IsQuestApplied(currentQuest.questID))
            {
                AppliedQuests.Instance.ApplyQuest(currentQuest);
                UpdateData(currentQuest);
            }
            else
            {
                AppliedQuests.Instance.CancleQuest(currentQuest);
                UpdateData(currentQuest);
            }
        }
    }

    private void Update()
    {
        if (InputM.GetAxisDown(KAction.TabsHorizontal))
        {
            if (InputM.GetAxisRaw(KAction.TabsHorizontal) < 0)
            {
                questList.Enable();
                Disable();
            }
        }
        for (int i = 0; i < items.Count; i++)
        {
            items[i].over = upDownUI.selectedIndex == i ? ButtonEffect.ActionType.Over : ButtonEffect.ActionType.None;
        }
    }

    public void Clear()
    {
        currentQuest = null;
        targetName.text = "";
        targetSystem.text = "";
        rewardText.text = "";
        jumpsCount.text = "";
    }
    public void UpdateData(Quest quest)
    {
        currentQuest = quest;
        var last = quest.GetLastQuestPath();
        targetName.text = "Target: " + last.targetName;
        targetSystem.text = "System: " + last.solarName;
        rewardText.text = "Path:\n";
        foreach (var names in quest.ConvertToStrings())
        {
            rewardText.text += names + ">\n";
        }
        jumpsCount.text = "Jumps count: " + quest.JumpsCount().ToString();
        if (AppliedQuests.Instance.IsQuestApplied(quest.questID))
        {
            buttonText.text = "Cancel";
        }
        else
        {
            buttonText.text = "Apply";
        }
    }
}
