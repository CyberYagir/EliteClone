using UnityEngine;

namespace Core.UI
{
    public class UpDownUI : MonoBehaviour
    {
        public int itemsCount, selectedIndex;
        public Event OnChangeSelected = new Event();
        public Event OnNavigateChange = new Event();

        private void Start()
        {
            selectedIndex = -1;
        }

        private void Update()
        {
            if (gameObject.active && itemsCount != 0)
            {
            
                if (InputM.GetAxisDown(KAction.TabsVertical))
                {
                    if (selectedIndex == -1)
                    {
                        selectedIndex = 0;
                    }
                    selectedIndex -= InputM.GetAxisRaw(KAction.TabsVertical);
                    if (selectedIndex < 0) selectedIndex = itemsCount - 1;
                    if (selectedIndex >= itemsCount) selectedIndex = 0;
                    OnNavigateChange.Run();
                }

                if (InputM.GetAxisDown(KAction.Select))
                {
                    if (selectedIndex == -1)
                    {
                        selectedIndex = 0;
                    }

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
            else
            {
                if (itemsCount == 0)
                    selectedIndex = -1;
            }
        }

        public void ForceChangeSelect(int newSelected)
        {
            selectedIndex = newSelected;
            OnNavigateChange.Run();
            OnChangeSelected.Run();
        }
    }
}
