using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class NavItem
{
    public ButtonEffect button;
    public Image image, icon;
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

    private float height;
    private void Awake()
    {
        height = item.GetComponent<RectTransform>().sizeDelta.y;
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
        objects = objects.Reverse().ToArray();
        for (int i = 0; i < objects.Length; i++)
        {
            var b = Instantiate(item, holder.transform);

            if (objects[i].transform.parent != null)
            {
                if (objects[i].transform.parent.GetComponent<WorldSpaceObject>())
                {
                    var rect = b.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(rect.sizeDelta.x * 0.95f, rect.sizeDelta.y);
                }
            }

            var navI = new NavItem();
            navI.button = b.GetComponent<ButtonEffect>();
            navI.spaceObject = objects[i];
            navI.image = objects[i].GetComponent<Image>();
            navI.name = b.transform.GetChild(0).GetComponent<TMP_Text>();
            navI.dist = b.transform.GetChild(1).GetComponent<TMP_Text>();
            navI.icon = b.transform.GetChild(2).GetComponent<Image>();
            navI.icon.sprite = objects[i].icon;
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
            holder.localPosition = Vector3.Lerp(holder.localPosition, new Vector3(holder.localPosition.x, (selectedIndex * height) - height, holder.localPosition.z), 10 * Time.deltaTime);
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
