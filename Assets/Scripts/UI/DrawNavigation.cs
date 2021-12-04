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
    public ButtonEffect Button;
    public Image Image, Icon;
    public TMP_Text Name, Dist;
    public WorldSpaceObject SpaceObject;
}
public class DrawNavigation : MonoBehaviour
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
            navI.Button = b.GetComponent<ButtonEffect>();
            navI.SpaceObject = objects[i];
            navI.Image = objects[i].GetComponent<Image>();
            navI.Name = b.transform.GetChild(0).GetComponent<TMP_Text>();
            navI.Dist = b.transform.GetChild(1).GetComponent<TMP_Text>();
            navI.Icon = b.transform.GetChild(2).GetComponent<Image>();
            navI.Icon.sprite = objects[i].icon;
            navI.Name.text = objects[i].transform.name;
            navI.Dist.text = objects[i].dist;
            b.SetActive(true);
            items.Add(navI);
        }
    }

    private void Update()
    {
        if (tabControl.Active)
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
                if (items[selectedIndex].SpaceObject != Player.inst.GetTarget())
                {
                    Player.inst.SetTarget(items[selectedIndex].SpaceObject);
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
                items[i].Dist.gameObject.SetActive(items[i].SpaceObject.isVisible);
                items[i].Dist.text = items[i].SpaceObject.dist;
            }
        }
    }
}
