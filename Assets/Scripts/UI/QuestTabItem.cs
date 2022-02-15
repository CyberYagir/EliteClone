using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class QuestTabItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text targetT, jumpToT;
    [SerializeField] private Transform rewardHolder, rewardItem;
    [SerializeField] private Transform requireHolder;

    public void Init(Quest questsQuest)
    {
        icon.sprite = QuestDataItem.GetData().mineType.Find(x => x.type == questsQuest.questType).icon;
        targetT.text = questsQuest.GetLastQuestPath().targetName;
        QuestPath first = questsQuest.pathToTarget;
        QuestPath questPath = first;
        QuestPath current = null;
        bool isOnPath = false;
        while (questPath != null)
        {
            if (!isOnPath)
            {
                isOnPath = questPath.solarName == PlayerDataManager.CurrentSolarSystem.name;
                if (isOnPath)
                {
                    current = questPath;
                    break;
                }
            }
            questPath = questPath.nextPath;
        }

        if (isOnPath)
            jumpToT.text = questsQuest.GetLastQuestPath().solarName == PlayerDataManager.CurrentSolarSystem.name ? "Arrival point" : "Next : " + current.nextPath.solarName;
        else
            jumpToT.text = $"Unown path [{first.solarName}]";

        UITweaks.ClearHolder(rewardHolder);
        UITweaks.ClearHolder(requireHolder);

        for (int i = 0; i < questsQuest.reward.rewardItems.Count; i++)
        {
            var it = Instantiate(rewardItem, rewardHolder).GetComponent<ItemUI>();
            it.Init(questsQuest.reward.rewardItems[i]);
            it.gameObject.SetActive(true);
        }
        for (int i = 0; i < questsQuest.toTransfer.Count; i++)
        {
            var it = Instantiate(rewardItem, requireHolder).GetComponent<ItemUI>();
            it.Init(questsQuest.toTransfer[i]);
            it.gameObject.SetActive(true);
        }
    }
}
