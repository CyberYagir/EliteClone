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
    [SerializeField] private TMP_Text rewardText, rewardTypeText, jumpsCount;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private Transform rewardsHolder, rewardItem;
    [SerializeField] private Transform transferHolder, transferFullInfo;
    
    
    private Quest currentQuest;
    
    private void Start()
    {
        (questList as QuestListUI).OnChangeSelected += UpdateData;
        upDownUI.OnChangeSelected += SelectButton;
        Player.OnSceneChanged += UpdateQuestInfo;
    }

    private void OnDestroy()
    {
        Player.OnSceneChanged -= UpdateQuestInfo;
    }

    void UpdateQuestInfo()
    {
        currentQuest = null;
        Clear();
        Disable();
    }

    private void SelectButton()
    {
        if (upDownUI.selectedIndex == 0)
        {
            if (!AppliedQuests.Instance.IsQuestApplied(currentQuest.questID))
            {
                if (!currentQuest.isComplited)
                {
                    AppliedQuests.Instance.ApplyQuest(currentQuest);
                    UpdateData(currentQuest);
                }
                else
                {
                    //Get Reward
                }
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
        rewardTypeText.text = "";
    }

    public void DrawItems(Transform holder, Transform item, List<Item> items)
    {
        foreach (Transform it in holder)
        {
            if (it.gameObject.active)
                Destroy(it.gameObject);
        }

        for (int i = 0; i < items.Count; i++)
        {
            var it = Instantiate(item, holder);
            it.GetComponent<ItemUI>().Init(items[i]);
            it.gameObject.SetActive(true);
        }
    }
    public void UpdateData(Quest quest)
    {
        currentQuest = quest;
        var last = quest.GetLastQuestPath();
        targetName.text = "Target: " + last.targetName;
        targetSystem.text = "System: " + last.solarName;
        rewardTypeText.text = "Reward: " + quest.reward.type;
        jumpsCount.text = "Jumps count: " + quest.JumpsCount() + "\n\nPath:\n";
        rewardText.text = "Reward: " + quest.reward.rewardItems.Count + " Items";

        DrawItems(rewardsHolder, rewardItem, quest.reward.rewardItems);

        transferFullInfo.gameObject.SetActive(quest.questType == Quest.QuestType.Transfer);
        if (quest.questType == Quest.QuestType.Transfer)
        {
            DrawItems(transferHolder, rewardItem, quest.toTransfer);
        }
        
        foreach (var names in quest.ConvertToStrings())
        {
            jumpsCount.text += names + ">\n";
        }
        
        if (AppliedQuests.Instance.IsQuestApplied(quest.questID))
        {
            if (!quest.isComplited)
            {
                buttonText.text = "Cancel";
            }
            else
            {
                if (!quest.isRewarded)
                {
                    buttonText.text = "Finish";
                }
                else
                {
                    buttonText.text = "Ended";
                }
            }
        }
        else
        {
            buttonText.text = "Apply";
        }
    }
}
