using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UI;
using UnityEngine;

public class CharacterList : BaseTabUI
{
    [SerializeField] private Transform holder;
    [SerializeField] private QuestListUI questList;
    [SerializeField] private QuesterItemUI item;
    [SerializeField] private ButtonActiveControlUI buttonsUI;
    
    private List<ButtonEffect> items = new List<ButtonEffect>();
    private BaseWindow baseWindow;
    public Event ChangeSelect = new Event();



    public void RedrawQuests()
    {
        if (WorldOrbitalStation.Instance != null)
        {
            var character = items[upDownUI.selectedIndex].GetComponent<QuesterItemUI>().GetCharacter();
            var quests = WorldOrbitalStation.Instance.quests.FindAll(x => x.quester == character);
            questList.UpdateQuests(quests);
        }
    }


    private void Start()
    {
        baseWindow = GetComponentInParent<BaseWindow>();
        WorldOrbitalStation.OnInit += UpdateList;
        upDownUI.OnChangeSelected += ChangeSelected;
        upDownUI.OnNavigateChange += ChangeSelected;
        ChangeSelect += RedrawQuests;
    }

    // private void OnDestroy()
    // {
    //     ChangeSelect -= RedrawQuests;
    // }

    public void ChangeSelected()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].over = upDownUI.selectedIndex == i ? ButtonEffect.ActionType.Over : ButtonEffect.ActionType.None;
        }
        ChangeSelect.Run();
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
            if (InputM.GetAxisRaw(KAction.TabsHorizontal) < 0)
            {
                buttonsUI.enabled = true;
                buttonsUI.SkipFrame();
                Disable();
            }
        }

        if (upDownUI.itemsCount != 0)
        {
            var rect = holder.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(0, upDownUI.selectedIndex * 255f), 10 * Time.deltaTime);  //255 - Высота итема магическим числом так как лень
        }
    }

    public void UpdateList()
    {
        UITweaks.ClearHolder(holder);
        
        items = new List<ButtonEffect>();
        WorldOrbitalStation.Instance.RemoveCharactersWithoutQuests();
        int id = 0;
        foreach (var character in WorldOrbitalStation.Instance.characters)
        {
            var it = Instantiate(item.gameObject, holder).GetComponent<QuesterItemUI>();
            it.InitQuesterItem(character.fraction, baseWindow.GetIcon(character.fraction).icon, character.firstName + " " + character.lastName, character, id);
            it.gameObject.SetActive(true);
            var bEffect = it.GetComponent<ButtonEffect>();
            items.Add(bEffect);
            if (WorldOrbitalStation.Instance.additionalCharacters.Contains(character.characterID))
            {
                bEffect.SetNoneColor(Color.green);
            }

            id++;
        }
        upDownUI.itemsCount = WorldOrbitalStation.Instance.characters.Count;

        ChangeSelected();
    }
}
