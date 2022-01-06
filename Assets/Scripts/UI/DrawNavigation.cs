using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
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
    [SerializeField] private UpDownUI updown;
    
    
    private UITabControl tabControl;

    private float height;
    private void Awake()
    {
        height = item.GetComponent<RectTransform>().sizeDelta.y;
        Player.OnSceneChanged += UpdateList;
        updown.OnChangeSelected += ChangeSelect;
        updown.OnNavigateChange += UpdateColors;
    }

    private void Start()
    {
        UpdateList();
    }

    private void UpdateList()
    {
        tabControl = GetComponentInParent<UITabControl>();
        
        UITweaks.ClearHolder(holder);
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

        updown.selectedIndex = 0;
        updown.itemsCount = items.Count;
    }


    public void ChangeSelect()
    {
        int selectedIndex = updown.selectedIndex;
        if (items[selectedIndex].SpaceObject != Player.inst.GetTarget())
        {
            Player.inst.SetTarget(items[selectedIndex].SpaceObject);
        }
        else
        {
            Player.inst.SetTarget(null);
        }
        UpdateColors();
    }

    public void UpdateColors()
    {
        int selectedIndex = updown.selectedIndex;
        for (int i = 0; i < items.Count; i++)
        {
            if (i == selectedIndex)
            {
                items[i].Button.over = ButtonEffect.ActionType.Over;
            }
            else if (items[i].SpaceObject == Player.inst.GetTarget() && items[i].SpaceObject != null)
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
    
    private void Update()
    {
        updown.enabled = tabControl.Active;
        
        if (tabControl.Active)
        {
            holder.localPosition = Vector3.Lerp(holder.localPosition, new Vector3(holder.localPosition.x, (updown.selectedIndex * height) - height, holder.localPosition.z), 10 * Time.deltaTime);
        }
    }
}
