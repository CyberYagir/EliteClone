using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownUI : MonoBehaviour
{
    public int itemsCount, selectedIndex;
    public event Action OnChangeSelected = delegate {  };
    public event Action OnNavigateChange = delegate {  };
    private void Update()
    {
        if (gameObject.active)
        {
            if (InputM.GetAxisDown(KAction.TabsVertical))
            {
                selectedIndex -= InputM.GetAxisRaw(KAction.TabsVertical);
                if (selectedIndex < 0) selectedIndex = itemsCount - 1;
                if (selectedIndex >= itemsCount) selectedIndex = 0;
                OnNavigateChange();
            }

            if (InputM.GetAxisDown(KAction.Select))
            {
                OnChangeSelected();
            }
        }
    }
}
