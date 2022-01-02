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

    public void Init(Quest questsQuest)
    {
        icon.sprite = QuestDataItem.GetData().mineType.Find(x => x.type == questsQuest.questType).icon;
        targetT.text = questsQuest.GetLastQuestPath().targetName;
        QuestPath first = questsQuest.pathToTarget;
        QuestPath questPath = first;
        bool isOnPath = false;
        while (!questPath.isLast)
        {
            questPath = questPath.nextPath;
            if (!isOnPath)
            {
                isOnPath = questPath.solarName == PlayerDataManager.CurrentSolarSystem.name;
            }
        }
    
        if (isOnPath)
            jumpToT.text = questPath.solarName == PlayerDataManager.CurrentSolarSystem.name ? "Arrival point" : "Next : " + questPath.solarName;
        else
            jumpToT.text = $"Unown path [{first.solarName}]";

        UITweaks.ClearHolder(rewardHolder);

        for (int i = 0; i < questsQuest.reward.rewardItems.Count; i++)
        {
            var it = Instantiate(rewardItem, rewardHolder).GetComponent<ItemUI>();
            it.Init(questsQuest.reward.rewardItems[i]);
            it.gameObject.SetActive(true);
        }
    }
}
