using System.Collections.Generic;
using Quests;
using UI;
using UnityEngine;

public class QuestListUI : BaseTabUI
{
    [SerializeField] private Transform item, holder;
    [SerializeField] private List<ButtonEffect> items = new List<ButtonEffect>();
    [SerializeField] private BaseTabUI characterList, questInfo;

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
            if (quests[i].questState != Quest.QuestComplited.Rewarded)
            {
                var questItem = Instantiate(item, holder);
                questItem.GetComponent<QuestItemUI>().Init(QuestDataItem.GetData().mineType.Find(x => x.type == quests[i].questType).icon, quests[i].questType.ToString(), i);
                questItem.gameObject.SetActive(true);
                items.Add(questItem.GetComponent<ButtonEffect>());
                count++;
            }
        }

        upDownUI.selectedIndex = 0;
        upDownUI.itemsCount = count;
    }
}
