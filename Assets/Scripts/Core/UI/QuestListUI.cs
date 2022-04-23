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
        public Event<Quest> OnChangeSelected = new Event<Quest>();
    

        private void Start()
        {
            upDownUI.OnChangeSelected += ChangeSelected;
            upDownUI.OnNavigateChange += ChangeSelected;
            Player.OnSceneChanged += () =>
            {
                characterList.Enable();
                Disable();
            };

            Player.inst.land.OnUnLand += Disable;
        }


        public void ChangeSelected()
        {
                for (int i = 0; i < items.Count; i++)
                {
                    items[i].over = upDownUI.selectedIndex == i ? ButtonEffect.ActionType.Over : ButtonEffect.ActionType.None;
                }

                OnChangeSelected.Run(questsList[upDownUI.selectedIndex]);
        }

        private void Update()
        {
            if (InputM.GetAxisDown(KAction.TabsHorizontal))
            {
                if (InputM.GetAxisRaw(KAction.TabsHorizontal) < 0)
                {
                    characterList.Enable();
                    Disable();
                }
                if (InputM.GetAxisRaw(KAction.TabsHorizontal) > 0)
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
    }
}