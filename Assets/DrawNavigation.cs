using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public List<NavItem> items = new List<NavItem>();
    public GameObject item, holder;
    public RectTransform content;
    public int selectedIndex;
    UITabControl tabControl;
    private void Start()
    {
        tabControl = GetComponentInParent<UITabControl>();
        for (int i = 0; i < SolarSystemGenerator.objects.Count; i++)
        {
            var b = Instantiate(item, holder.transform);
            var navI = new NavItem();
            navI.button = b.GetComponent<ButtonEffect>();
            navI.spaceObject = SolarSystemGenerator.objects[i];
            navI.image = SolarSystemGenerator.objects[i].GetComponent<Image>();
            navI.name = b.transform.GetChild(0).GetComponent<TMP_Text>();
            navI.dist = b.transform.GetChild(1).GetComponent<TMP_Text>();
            navI.name.text = SolarSystemGenerator.objects[i].transform.name;
            navI.dist.text = SolarSystemGenerator.objects[i].dist;
            b.SetActive(true);
            items.Add(navI);
        }
    }

    private void Update()
    {
        if (tabControl.active)
        {
            content.localPosition = new Vector3(content.localPosition.x, selectedIndex * 42, content.localPosition.z);
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
