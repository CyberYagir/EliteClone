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


        private WorldDataHandler worldHandler;
        public void Init(Quest questsQuest)
        {

            worldHandler = PlayerDataManager.Instance.WorldHandler;
            
            var questIcon = WorldDataItem.Quests.IconByID(questsQuest.questType);
            if (questsQuest.questType == -1)
            {
                questIcon = WorldDataItem.GetData().playerQuest;
            }
            icon.sprite = questIcon;

            targetT.text = questsQuest.targetStructure;




            if (questsQuest.CurrentPath.Count == 0 || questsQuest.isOnLoading)
            {
                if (!questsQuest.IsEmptyQuest())
                {
                    jumpToT.text = "Calculating...";
                }
                else
                {
                    
                    jumpToT.text = "<color=#FFFFFF10>Empty";
                }
            }
            else
            {
                if (worldHandler.CurrentSolarSystem.name == questsQuest.targetSolar)
                {
                    jumpToT.text = "Arrival point";
                }
                else
                {
                    for (int i = 0; i < questsQuest.CurrentPath.Count-1; i++)
                    {
                        if (questsQuest.CurrentPath[i] == worldHandler.CurrentSolarSystem.name)
                        {
                            jumpToT.text = "Next : " + questsQuest.CurrentPath[i + 1];
                            break;
                        }
                    }
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
