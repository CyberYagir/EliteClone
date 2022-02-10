using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitTabs : MonoBehaviour
{
    [SerializeField] private int tabIndex;
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private ButtonEffect[] buttons;

    public Event OnChangeTab = new Event();
    
    private void Start()
    {
        ChangeTab(0);
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].over = i == tabIndex ? ButtonEffect.ActionType.Over : ButtonEffect.ActionType.None;
        }
    }

    public void ChangeTab(int index)
    {
        tabIndex = index;
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].gameObject.SetActive(i == tabIndex);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(tabs[index].GetComponent<RectTransform>());
        OnChangeTab.Run();
    }
}
