using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public abstract class DrawNavList : MonoBehaviour
    {
        [SerializeField] protected List<NavItem> items = new List<NavItem>();
        [SerializeField] protected GameObject item;
        [SerializeField] protected RectTransform holder;
        [SerializeField] protected UpDownUI updown;


        protected UITabControl tabControl;

        protected float height;

        protected virtual void Awake()
        {
            height = item.GetComponent<RectTransform>().sizeDelta.y;
            Player.Player.OnSceneChanged += UpdateList;
            updown.OnChangeSelected += ChangeSelect;
            updown.OnNavigateChange += UpdateColors;
        }

        protected virtual void Start()
        {
            UpdateList();
        }

        protected void UpdateList()
        {
            tabControl = GetComponentInParent<UITabControl>();
            UITweaks.ClearHolder(holder);
            GameObject selected = null;
            if (items.Count != 0)
            {
                try
                {
                    selected = items[updown.selectedIndex].SpaceObject.gameObject;
                }
                catch (Exception e)
                {
                    // ignored
                }
            }

            items = new List<NavItem>();
            RedrawList();
            if (selected != null)
            {
                updown.selectedIndex = items.FindIndex(x => x.SpaceObject != null && x.SpaceObject.gameObject == selected);
            }
            else
            {
                updown.selectedIndex = 0;
            }
            updown.itemsCount = items.Count;
            UpdateWithoutLerp();
        }

        public virtual void RedrawList()
        {
       
        }


        public void ChangeSelect()
        {
            int selectedIndex = updown.selectedIndex;
            if (selectedIndex != -1)
            {
                if (items[selectedIndex].SpaceObject != Player.Player.inst.GetTarget())
                {
                    Player.Player.inst.SetTarget(items[selectedIndex].SpaceObject);
                }
                else
                {
                    Player.Player.inst.SetTarget(null);
                }

                UpdateColors();
            }
        }

        public void UpdateColors()
        {
            int selectedIndex = updown.selectedIndex;
            if (selectedIndex != -1)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].SpaceObject != null)
                    {
                        if (i == selectedIndex)
                        {
                            items[i].Button.over = ButtonEffect.ActionType.Over;
                        }
                        else if (items[i].SpaceObject == Player.Player.inst.GetTarget() && items[i].SpaceObject != null)
                        {
                            items[i].Button.over = ButtonEffect.ActionType.Selected;
                        }
                        else
                        {
                            items[i].Button.over = ButtonEffect.ActionType.None;
                        }
                        //items[i].Dist.gameObject.SetActive(items[i].SpaceObject.isVisible);
                        //items[i].Dist.text = items[i].SpaceObject.dist;
                    }
                }
            }
        }

        public void UpdateWithoutLerp()
        {
            int selectedIndex = updown.selectedIndex;
            if (selectedIndex != -1)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    if (items[i].SpaceObject != null)
                    {
                        if (i == selectedIndex)
                        {
                            items[i].Button.over = ButtonEffect.ActionType.Over;
                        }
                        else if (items[i].SpaceObject == Player.Player.inst.GetTarget() && items[i].SpaceObject != null)
                        {
                            items[i].Button.over = ButtonEffect.ActionType.Selected;
                        }
                        else
                        {
                            items[i].Button.over = ButtonEffect.ActionType.None;
                        }
                        items[i].Button.WithoutLerp();
                        //items[i].Dist.gameObject.SetActive(items[i].SpaceObject.isVisible);
                        //items[i].Dist.text = items[i].SpaceObject.dist;
                    }
                }
            }
        }

        private void Update()
        {
            updown.enabled = tabControl.Active;

            if (tabControl.Active)
            {
                holder.localPosition = Vector3.Lerp(holder.localPosition, new Vector3(holder.localPosition.x, (updown.selectedIndex * height) - height, holder.localPosition.z), 10 * Time.deltaTime);
            }
        }
    }
}