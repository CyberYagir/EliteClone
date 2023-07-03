using System.Collections.Generic;
using Core.Game;
using Core.Location;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.UI
{
    public class QuestListUI : BaseTabUI
    {
        [SerializeField] private Transform item, holder;
        [SerializeField] private List<ButtonEffect> items = new List<ButtonEffect>();
        [SerializeField] public BaseTabUI characterList, questInfo;

        private List<Quest> questsList;
        private bool skipFrame = false;
        
        public Event<Quest> OnChangeSelected = new Event<Quest>();


        public int QuestsCount => questsList == null ? 0 : questsList.Count;

        public override void Init()
        {
            base.Init();
            
            upDownUI.OnChangeSelected += ChangeSelected;
            upDownUI.OnNavigateChange += ChangeSelected;
            Player.OnSceneChanged += () =>
            {
                characterList.Enable();
                Disable();
            };

            WorldDataHandler.ShipPlayer.land.OnUnLand += Disable;
        }



        public void ChangeSelected()
        {
            if (questsList.Count == 0) return;
            for (int i = 0; i < items.Count; i++)
            {
                items[i].over = upDownUI.selectedIndex == i ? ButtonEffect.ActionType.Over : ButtonEffect.ActionType.None;
            }

            if (upDownUI.selectedIndex <= -1 || questsList.Count <= upDownUI.selectedIndex)
            {
                upDownUI.selectedIndex = 0;
            }

            OnChangeSelected.Run(questsList[upDownUI.selectedIndex]);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (skipFrame)
            {
                skipFrame = false;
                return;
            }

            if (upDownUI.selectedIndex == -1)
            {
                upDownUI.ForceChangeSelect(0);
            }
            if (InputService.GetAxisDown(KAction.TabsHorizontal))
            {
                if (InputService.GetAxisRaw(KAction.TabsHorizontal) < 0)
                {
                    characterList.Enable();
                    Disable();
                }

                if (InputService.GetAxisRaw(KAction.TabsHorizontal) > 0)
                {
                    questInfo.Enable();
                    Disable();
                }
            }
        }

        public void UpdateQuests(List<Quest> quests)
        {
            questsList = quests;
        
            UITweaks.ClearHolder(holder);
            var solarShips = FindObjectOfType<SolarSystemShips>();
            List<Quest> questsToRemove = new List<Quest>();
            for (int i = 0; i < quests.Count; i++)
            {
                var questData = WorldDataItem.Quests.ByID(quests[i].questType);
                if (questData.typeName == "Kill")
                {
                    var targetID = (int) quests[i].keyValues["BotTarget"];
                    if (solarShips != null)
                    {
                        if (solarShips.IsDead(targetID) && AppliedQuests.Instance.quests.Find(x=>x.questID == quests[i].questID) == null)
                        {
                            questsToRemove.Add(quests[i]);
                        }
                    }
                }
            }

            for (int i = 0; i < questsToRemove.Count; i++)
            {
                quests.Remove(questsToRemove[i]);
            }
            
            items = new List<ButtonEffect>();
            int count = 0;
            for (int i = 0; i < quests.Count; i++)
            {
                var findQuest = AppliedQuests.Instance.quests.Find(x => x.questID == quests[i].questID);
                if (findQuest == null || findQuest.questState != Quest.QuestCompleted.Completed)
                {
                    var questItem = Instantiate(item, holder);
                    var questData = WorldDataItem.Quests.ByID(quests[i].questType);
                    
                    questItem.GetComponent<QuestItemUI>().Init(questData.icon, questData.typeName, count);
                    questItem.gameObject.SetActive(true);
                    items.Add(questItem.GetComponent<ButtonEffect>());
                    count++;
                }
            }

            upDownUI.selectedIndex = 0;
            upDownUI.itemsCount = count;
        }

        public void SkipFrame()
        {
            skipFrame = true;
        }
    }
}