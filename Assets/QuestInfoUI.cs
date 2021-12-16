using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInfoUI : BaseTab
{
    [SerializeField] private List<ButtonEffect> items;
    [SerializeField] private BaseTab questList;
    private void Update()
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
