using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

public class QuestListUI : BaseTab
{
    [SerializeField] private Transform item, holder;
    [SerializeField] private List<MineTypes> mineType;
    [SerializeField] private List<ButtonEffect> items = new List<ButtonEffect>();
    [SerializeField] private BaseTab characterList, questInfo;
    [System.Serializable]
    public class MineTypes
    {
        public Quest.QuestType type;
        public Sprite icon;
    }

    private void Start()
    {
        upDownUI.OnChangeSelected += ChangeSelected;
        upDownUI.OnNavigateChange += ChangeSelected;
    }


    public void ChangeSelected()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].over = upDownUI.selectedIndex == i ? ButtonEffect.ActionType.Over : ButtonEffect.ActionType.None;
        }
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
        foreach (Transform items in holder)
        {
            if (items.gameObject.active)
                Destroy(items.gameObject);
        }
        items = new List<ButtonEffect>();
        for (int i = 0; i < quests.Count; i++)
        {
            var questItem =  Instantiate(item, holder);
            questItem.GetComponent<QuestItemUI>().Init(mineType.Find(x=>x.type == quests[i].questType).icon, quests[i].questType.ToString());
            questItem.gameObject.SetActive(true);
            items.Add(questItem.GetComponent<ButtonEffect>());
        }

        upDownUI.selectedIndex = 0;
        upDownUI.itemsCount = items.Count;
    }
}
