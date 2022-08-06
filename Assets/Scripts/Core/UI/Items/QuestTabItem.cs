using System;
using Core.Game;
using Core.Location;
using Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class QuestTabItem : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text targetT, jumpToT, QuestTextT;
        [SerializeField] private Transform rewardHolder, rewardItem;
        [SerializeField] private Transform requireHolder, requireItemList, rewardItemList, questTextHolder, rewardText;

        public void Init(Quest questsQuest)
        {
            var questIcon = WorldDataItem.Quests.IconByID(questsQuest.questType);
            if (questsQuest.questType == -1)
            {
                questIcon = WorldDataItem.GetData().playerQuest;
            }
            icon.sprite = questIcon;
            var path = questsQuest.GetLastQuestPath();
            if (path != null)
            {
                targetT.text =  path.targetName;
            }

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

            if (questsQuest.GetLastQuestPath() != null)
            {
                if (isOnPath)
                {
                    jumpToT.text = questsQuest.GetLastQuestPath().solarName == PlayerDataManager.CurrentSolarSystem.name ? "Arrival point" : "Next : " + current.nextPath.solarName;
                }
                else
                {
                    jumpToT.text = $"Unown path [{first.solarName}]";
                }
            }

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
            
            requireItemList.gameObject.SetActive(questsQuest.toTransfer.Count != 0 && questsQuest.questType != -1);
            rewardItemList.gameObject.SetActive(questsQuest.questType != -1);
            rewardText.gameObject.SetActive(questsQuest.questType != -1);
            questTextHolder.gameObject.SetActive(questsQuest.toTransfer.Count == 0 || questsQuest.questType == -1);
            if (questsQuest.toTransfer.Count == 0 || questsQuest.questType == -1)
            {
                try
                {
                    QuestTextT.text = questsQuest.keyValues["Text"].ToString();
                }
                catch (Exception e)
                {
                }
            }
        }

        private void Start()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetChild(0).GetComponent<RectTransform>());
        }
    }
}
