using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class NavItem
{
    public ButtonEffect button;
    public Image image;
    public TMP_Text name, dist;
    public WorldSpaceObject spaceObject;
}
public class DrawNavigation : MonoBehaviour
{
    [SerializeField] List<NavItem> items = new List<NavItem>();
    [SerializeField] GameObject item;
    [SerializeField] RectTransform holder;
    [SerializeField] int selectedIndex;
    UITabControl tabControl;

    private void Awake()
    {
        Player.OnSceneChanged += UpdateList;
    }

    private void OnDestroy()
    {
        Player.OnSceneChanged -= UpdateList;
    }

    private void UpdateList()
    {
        tabControl = GetComponentInParent<UITabControl>();
        foreach (Transform navitem in holder.transform)
        {
            if (navitem.gameObject.activeSelf)
            {
                Destroy(navitem.gameObject);
            }
        }

        items = new List<NavItem>();
        var objects = FindObjectsOfType<WorldSpaceObject>();
        for (int i = 0; i < objects.Length; i++)
        {
            var b = Instantiate(item, holder.transform);
            var navI = new NavItem();
            navI.button = b.GetComponent<ButtonEffect>();
            navI.spaceObject = objects[i];
            navI.image = objects[i].GetComponent<Image>();
            navI.name = b.transform.GetChild(0).GetComponent<TMP_Text>();
            navI.dist = b.transform.GetChild(1).GetComponent<TMP_Text>();
            navI.name.text = objects[i].transform.name;
            navI.dist.text = objects[i].dist;
            b.SetActive(true);
            items.Add(navI);
        }
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
                if (items[selectedIndex].spaceObject != Player.inst.GetTarget())
                {
                    Player.inst.SetTarget(items[selectedIndex].spaceObject);
                }
                else
                {
                    Player.inst.SetTarget(null);
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
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
                items[i].dist.gameObject.SetActive(items[i].spaceObject.isVisible);
                items[i].dist.text = items[i].spaceObject.dist;
            }
        }
    }
}
