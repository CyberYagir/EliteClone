using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTab: MonoBehaviour
{
    [SerializeField] public GameObject enableOverlay;
    [SerializeField] public UpDownUI upDownUI;
    
    public void Enable()
    {
        this.enabled = true;
        upDownUI.enabled = true;
    }
    public void Disable()
    {
        this.enabled = false;
        upDownUI.enabled = false;
    }
}

public class CharacterList : BaseTab
{
    [SerializeField] private Transform holder;
    [SerializeField] private QuestListUI questList;
    [SerializeField] private QuesterItemUI item;
    private List<ButtonEffect> items = new List<ButtonEffect>();
    private BaseWindow baseWindow;

    public event Action ChangeSelect = delegate {  };



 

    public void RedrawQuests()
    {
        var character = items[upDownUI.selectedIndex].GetComponent<QuesterItemUI>().GetCharacter();
        var quests = WorldOrbitalStation.Instance.quests.FindAll(x => x.quester == character);
        questList.UpdateQuests(quests);
    }


    private void Start()
    {
        baseWindow = GetComponentInParent<BaseWindow>();
        WorldOrbitalStation.OnInit += UpdateList;
        upDownUI.OnChangeSelected += ChangeSelected;
        upDownUI.OnNavigateChange += ChangeSelected;
        ChangeSelect += RedrawQuests;
    }

    public void ChangeSelected()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].over = upDownUI.selectedIndex == i ? ButtonEffect.ActionType.Over : ButtonEffect.ActionType.None;
        }
        ChangeSelect();
    }

    private void Update()
    {
        if (InputM.GetAxisDown(KAction.TabsHorizontal))
        {
            if (InputM.GetAxisRaw(KAction.TabsHorizontal) > 0)
            {
                questList.ChangeSelected();
                questList.Enable();
                Disable();
            }
        }
    }

    public void UpdateList()
    {
        foreach (Transform tr in holder)
        {
            if (tr.gameObject.activeSelf)
                Destroy(tr.gameObject);
        }

        items = new List<ButtonEffect>();
        foreach (var character in WorldOrbitalStation.Instance.characters)
        {
            var it = Instantiate(item.gameObject, holder).GetComponent<QuesterItemUI>();
            it.InitQuesterItem(character.fraction, baseWindow.GetIcon(character.fraction).icon, character.firstName + " " + character.lastName, character);
            it.gameObject.SetActive(true);
            items.Add(it.GetComponent<ButtonEffect>());
        }
        upDownUI.itemsCount = WorldOrbitalStation.Instance.characters.Count;
        ChangeSelected();
    }
}
