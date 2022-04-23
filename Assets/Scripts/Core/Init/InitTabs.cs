using Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Init
{
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

        public Transform ChangeTab(int index)
        {
            tabIndex = index;
            Transform tab = null;
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].gameObject.SetActive(i == tabIndex);
                if (i == tabIndex) tab = tabs[i].transform;
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(tabs[index].GetComponent<RectTransform>());
            OnChangeTab.Run();
            return tab;
        }
        
        public void ChangeTab(float index)
        {
            tabIndex = (int)index;
            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].gameObject.SetActive(i == tabIndex);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(tabs[tabIndex].GetComponent<RectTransform>());
            OnChangeTab.Run();
        }
    }
}
