using System.Collections.Generic;
using Core.Game;
using Core.Location;
using Core.Player;
using Core.Systems;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class QuestInfoUI : BaseTabUI
    {
        [SerializeField] private List<ButtonEffect> items;
        [SerializeField] private BaseTabUI questList;
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
            Player.Player.OnSceneChanged += UpdateQuestInfo;
            Player.Player.inst.land.OnUnLand += Disable;
        }

        // private void OnDestroy()
        // {
        //     Player.OnSceneChanged -= UpdateQuestInfo;
        // }

        void UpdateQuestInfo()
        {
            currentQuest = null;
            Clear();
            Disable();
        }

        public void AddQuest()
        {
            AppliedQuests.Instance.ApplyQuest(currentQuest);
            UpdateData(currentQuest);
        }


        public void SelectButton()
        {
            if (Player.Player.inst.land.isLanded)
            {
                if (upDownUI.selectedIndex == 0)
                {
                    if (!AppliedQuests.Instance.IsQuestApplied(currentQuest.questID))
                    {
                        if (currentQuest.questType == Quest.QuestType.Transfer)
                        {
                            if (currentQuest.questState != Quest.QuestComplited.Complited)
                            {
                                AddQuest();
                            }
                        }else if (currentQuest.questType == Quest.QuestType.Mine)
                        {
                            if (currentQuest.questState == Quest.QuestComplited.Complited || currentQuest.questState == Quest.QuestComplited.None)
                            {
                                AddQuest();
                            }
                        }
                    }
                    else
                    {
                        if (currentQuest.questState != Quest.QuestComplited.Complited && currentQuest.questState != Quest.QuestComplited.Rewarded)
                        {
                            AppliedQuests.Instance.CancelQuest(currentQuest);
                            UpdateData(currentQuest);
                        }
                        else if (currentQuest.questState == Quest.QuestComplited.Complited)
                        {
                            if (currentQuest.questType == Quest.QuestType.Transfer || currentQuest.questType == Quest.QuestType.Mine)
                            {
                                if (Player.Player.inst.cargo.ContainItems(currentQuest.toTransfer))
                                {
                                    Player.Player.inst.cargo.RemoveItems(currentQuest.toTransfer);
                                    if (Player.Player.inst.cargo.AddItems(currentQuest.reward.rewardItems))
                                    {
                                        AppliedQuests.Instance.FinishQuest(currentQuest.questID);
                                        UpdateData(currentQuest);
                                        GetComponentInParent<BaseWindow>().RedrawAll();
                                    }
                                    else
                                    {
                                        Player.Player.inst.cargo.AddItems(currentQuest.toTransfer);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (Player.Player.inst.land.isLanded)
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
            UITweaks.ClearHolder(holder);
        
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
            quest.CheckIsQuestCompleted();
            targetName.text = "Target: " + last.targetName;
            targetSystem.text = "System: " + last.solarName;
            rewardTypeText.text = "Reward: " + quest.reward.type;
        
            if (quest.questType == Quest.QuestType.Transfer)
            {
                jumpsCount.enabled = true;
                jumpsCount.text = "Jumps count: " + quest.JumpsCount() + "\n\nPath:\n";
            }
            else
            {
                jumpsCount.enabled = false;
            }
            rewardText.text = "Reward: " + quest.reward.rewardItems.Count + " Items";

            DrawItems(rewardsHolder, rewardItem, quest.reward.rewardItems);
            transferFullInfo.gameObject.SetActive(quest.questType == Quest.QuestType.Transfer || quest.questType == Quest.QuestType.Mine);
        
        
            if (transferFullInfo.gameObject.active)
            {
                DrawItems(transferHolder, rewardItem, quest.toTransfer);
            }
        
            foreach (var names in quest.ConvertToStrings())
            {
                jumpsCount.text += names + ">\n";
            }
            buttonText.transform.parent.gameObject.SetActive(true);
            if (AppliedQuests.Instance.IsQuestApplied(quest.questID))
            {
                if (quest.questState == Quest.QuestComplited.None)
                {
                    buttonText.text = "";
                    TransferButton(quest);
                    MineButton(quest);
                }
                else
                {
                    if (quest.questState != Quest.QuestComplited.Rewarded)
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

        public void TransferButton(Quest quest)
        {
            if (quest.questType == Quest.QuestType.Transfer)
            {
                if (WorldOrbitalStation.Instance.transform.name == quest.GetLastQuestPath().targetName)
                {
                    buttonText.text = "Items to transfer missing";
                }
                else if (WorldOrbitalStation.Instance.transform.name == quest.appliedStation)
                {
                    if (quest.IsHaveAllItems())
                    {
                        buttonText.text = "Cancel";
                    }
                    else
                    {
                        buttonText.text = "Items to transfer missing [cant cancel]";
                    }
                }
                else
                {
                    buttonText.transform.parent.gameObject.SetActive(false);
                }
            }

        }

        public void MineButton(Quest quest)
        {
            if (quest.questType == Quest.QuestType.Mine)
            {
                if (WorldOrbitalStation.Instance.transform.name == quest.appliedStation)
                {
                    if (quest.IsHaveAllItems() && quest.questState == Quest.QuestComplited.Complited)
                    {
                        buttonText.text = "Finish";// хуй, после выполнения квеста с майн он остаётся и его можно взять
                    }
                    else
                    {
                        buttonText.text = "Cancel";
                    }
                }
            }
        }
    }
}
