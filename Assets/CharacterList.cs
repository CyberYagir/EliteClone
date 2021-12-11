using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : MonoBehaviour
{
    [SerializeField] private QuesterItemUI item;
    [SerializeField] private Transform holder;
    [SerializeField] private UpDownUI upDownUI;
    [SerializeField] private QuestListUI questList;
    private List<ButtonEffect> items = new List<ButtonEffect>();
    private BaseWindow baseWindow;

    public event Action ChangeSelect = delegate {  };


    private void Start()
    {
        baseWindow = GetComponentInParent<BaseWindow>();
        WorldOrbitalStation.OnInit += UpdateList;
        upDownUI.OnChangeSelected += ChangeSelected;
        upDownUI.OnNavigateChange += ChangeSelected;
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
        if (InputM.GetAxisRaw(KAction.TabsHorizontal) > 0)
        {
            questList.Enable();
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
    }
}
