using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class DrawContacts : MonoBehaviour  
{
    [SerializeField] private List<NavItem> items = new List<NavItem>();
    [SerializeField] private GameObject item;
    [SerializeField] private RectTransform holder;
    [SerializeField] private int selectedIndex;
    private UITabControl tabControl;
    private float height;

    private void Awake()
    {
        height = item.GetComponent<RectTransform>().sizeDelta.y;
    }

    private void UpdateList()
    {
        tabControl = GetComponentInParent<UITabControl>();
        
        UITweaks.ClearHolder(holder);

        items = new List<NavItem>();
        var objects = FindObjectsOfType<ContactObject>();
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].transform.tag != "Player")
            {
                var b = Instantiate(item, holder.transform);
                var navI = new NavItem();
                navI.Button = b.GetComponent<ButtonEffect>();
                navI.SpaceObject = objects[i].GetComponent<WorldSpaceObject>();
                navI.Image = objects[i].GetComponent<Image>();
                navI.Name = b.transform.GetComponentInChildren<TMP_Text>();
                navI.Icon = b.transform.GetChild(1).GetComponent<Image>();
                navI.Icon.sprite = objects[i].icon;
                navI.Name.text = objects[i].transform.name;
                b.SetActive(true);
                items.Add(navI);
            }
        }
    }

    //ЛЕГАСИ ПЕРЕПИСАТЬ ПОД UPDOWNUI, чтобы было как в DrawNavigation
    
    private void OnEnable()
    {
        UpdateList();
    }
    private void Update()
    {
        if (tabControl.Active)
        {
            holder.localPosition = new Vector3(holder.localPosition.x, selectedIndex * height, holder.localPosition.z);
            if (InputM.GetAxisDown(KAction.TabsVertical))
            {
                selectedIndex -= InputM.GetAxisRaw(KAction.TabsVertical);
                if (selectedIndex <= 0) selectedIndex = items.Count - 1;
                if (selectedIndex >= items.Count) selectedIndex = 0;
            }

            if (InputM.GetAxisDown(KAction.Select))
            {
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].SpaceObject == null)
                {
                    items.RemoveAt(i);
                    UpdateList();
                    return;
                }
                if (i == selectedIndex)
                {
                    items[i].Button.over = ButtonEffect.ActionType.Over;
                }
                else if (items[i].SpaceObject == Player.inst.GetTarget())
                {
                    items[i].Button.over = ButtonEffect.ActionType.Selected;
                }
                else
                {
                    items[i].Button.over = ButtonEffect.ActionType.None;
                }
            }
        }
    }
}
