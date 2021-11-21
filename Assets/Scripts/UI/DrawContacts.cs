using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawContacts : MonoBehaviour
{
    [SerializeField] List<NavItem> items = new List<NavItem>();
    [SerializeField] GameObject item;
    [SerializeField] RectTransform holder;
    [SerializeField] int selectedIndex;
    UITabControl tabControl;

    private void UpdateList()
    {
        tabControl = GetComponentInParent<UITabControl>();
        foreach (Transform it in holder.transform)
        {
            if (it.gameObject.active)
            {
                Destroy(it.gameObject);
            }
        }

        items = new List<NavItem>();
        var objects = FindObjectsOfType<ContactObject>();
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].transform.tag != "Player")
            {
                var b = Instantiate(item, holder.transform);
                var navI = new NavItem();
                navI.button = b.GetComponent<ButtonEffect>();
                navI.spaceObject = objects[i].GetComponent<WorldSpaceObject>();
                navI.image = objects[i].GetComponent<Image>();
                navI.name = b.transform.GetComponentInChildren<TMP_Text>();
                navI.name.text = objects[i].transform.name;
                b.SetActive(true);
                items.Add(navI);
            }
        }
    }

    private void OnEnable()
    {
        UpdateList();
    }
    private void Update()
    {
        if (tabControl.active)
        {
            holder.localPosition = new Vector3(holder.localPosition.x, selectedIndex * 42, holder.localPosition.z);
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
                if (items[i].spaceObject == null)
                {
                    items.RemoveAt(i);
                    UpdateList();
                    return;
                }
                if (i == selectedIndex)
                {
                    items[i].button.over = ButtonEffect.ActionType.Over;
                }
                else if (items[i].spaceObject == Player.inst.GetTarget())
                {
                    items[i].button.over = ButtonEffect.ActionType.Selected;
                }
                else
                {
                    items[i].button.over = ButtonEffect.ActionType.None;
                }
            }
        }
    }
}
