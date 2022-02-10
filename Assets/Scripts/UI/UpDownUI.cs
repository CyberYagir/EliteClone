using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownUI : MonoBehaviour
{
    public int itemsCount, selectedIndex;
    public Event OnChangeSelected = new Event();
    public Event OnNavigateChange = new Event();
    private void Update()
    {
        if (gameObject.active && itemsCount != 0)
        {
            if (InputM.GetAxisDown(KAction.TabsVertical))
            {
                selectedIndex -= InputM.GetAxisRaw(KAction.TabsVertical);
                if (selectedIndex < 0) selectedIndex = itemsCount - 1;
                if (selectedIndex >= itemsCount) selectedIndex = 0;
                OnNavigateChange.Run();
            }

            if (InputM.GetAxisDown(KAction.Select))
            {
                if (itemsCount <= selectedIndex)
                {
                    selectedIndex = itemsCount - 1;
                }

                if (itemsCount != 0)
                {
                    OnChangeSelected.Run();
                }
            }
        }
    }

    public void ForceChangeSelect(int newSelected)
    {
        selectedIndex = newSelected;
        OnNavigateChange.Run();
        OnChangeSelected.Run();
    }
}
