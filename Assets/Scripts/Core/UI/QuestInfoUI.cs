using System.Collections.Generic;
using Core.Game;
using Core.Location;
using Core.PlayerScripts;
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
        [SerializeField] private TMP_Text buttonText, keysText;
        [SerializeField] private Transform rewardsHolder, rewardItem;
        [SerializeField] private Transform transferHolder, transferFullInfo;
    
    
        private Quest currentQuest;
    
        private void Start()
        {
            (questList as QuestListUI).OnChangeSelected += UpdateData;
            upDownUI.OnChangeSelected += SelectButton;
            Player.OnSceneChanged += UpdateQuestInfo;
            Player.inst.land.OnUnLand += Disable;
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
            if (Player.inst.land.isLanded)
            {
                if (upDownUI.selectedIndex == 0 && currentQuest != null)
                {
                    if (!AppliedQuests.Instance.IsQuestApplied(currentQuest.questID))
                    {
                        if (currentQuest.questState != Quest.QuestCompleted.Completed)
                        {
                            AddQuest();
                        }
                    }
                    else //Applied Quest
                    {
                        currentQuest.CheckIsQuestCompleted();
                        if (currentQuest.questState != Quest.QuestCompleted.Completed && currentQuest.questState != Quest.QuestCompleted.Rewarded)
                        {
                            AppliedQuests.Instance.CancelQuest(currentQuest);
                            UpdateData(currentQuest);
                        }
                        else if (currentQuest.questState == Quest.QuestCompleted.Completed)
                        {
                            currentQuest.OnFinish();
                            if (currentQuest.questState == Quest.QuestCompleted.Rewarded)
                            {
                                UpdateData(currentQuest);
                                GetComponentInParent<BaseWindow>().RedrawAll();
                                Player.inst.quests.OnChangeQuests.Run();
                                var list = (questList as QuestListUI);
                                (list.characterList as CharacterList).RedrawQuests();
                                Clear();
                            }
                        }
                    }
                }
            }
        }

        private void Update()
        {
            if (Player.inst.land.isLanded)
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
            keysText.text = "";
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

        public void SetTexts(QuestPath last, Quest quest)
        {
            targetName.text = "Target: " + last.targetName;
            targetSystem.text = "System: " + last.solarName;
            rewardTypeText.text = "Reward: " + quest.reward.type;

            if (quest.keyValues.Count != 0 && quest.toTransfer.Count == 0 && quest.keyValues.ContainsKey("Text"))
            {
                keysText.gameObject.SetActive(true);
                keysText.text = "Quest Data: \n" + quest.keyValues["Text"];
            }
            else
            {
                keysText.gameObject.SetActive(false);
            }
            
            if (currentQuest.IsTypeQuest("Transfer"))
            {
                jumpsCount.enabled = true;
                jumpsCount.text = "Jumps count: " + quest.JumpsCount() + "\n\nPath:\n";
            }
            else
            {
                jumpsCount.enabled = false;
            }
            rewardText.text = "Reward: " + quest.reward.rewardItems.Count + " Items";

        }

        public void ReformUI(Quest quest)
        {
            DrawItems(rewardsHolder, rewardItem, quest.reward.rewardItems);
            transferFullInfo.gameObject.SetActive(currentQuest.IsTypeQuest("Transfer") || currentQuest.IsTypeQuest("Mine"));
            if (transferFullInfo.gameObject.active)
            {
                DrawItems(transferHolder, rewardItem, quest.toTransfer);
            }
        
            foreach (var names in quest.ConvertToStrings())
            {
                jumpsCount.text += names + ">\n";
            }
            buttonText.transform.parent.gameObject.SetActive(true);

        }
        
        public void UpdateData(Quest quest)
        {
            currentQuest = quest;
            var last = quest.GetLastQuestPath();
            quest.CheckIsQuestCompleted();
            SetTexts(last, quest);
            ReformUI(quest);
            
            
            if (AppliedQuests.Instance.IsQuestApplied(quest.questID))
            {
                if (quest.questState == Quest.QuestCompleted.None)
                {
                    buttonText.text = "";
                    quest.GetButtonText();
                    buttonText.text = quest.buttonText;
                }
                else
                {
                    if (quest.questState != Quest.QuestCompleted.Rewarded)
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
}
