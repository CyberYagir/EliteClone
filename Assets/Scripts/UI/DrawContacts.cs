using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class DrawContacts : DrawNavList
{
    protected override void Start()
    {
        base.Start();
        Player.inst.targets.ContactsChanges += UpdateList;
    }

    public override void RedrawList()
    {
        base.RedrawList();
        var objects = Player.inst.targets.contacts;
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].transform.CompareTag("Player"))
            {
                var b = Instantiate(item, holder.transform);
                var navI = new NavItem();
                navI.Button = b.GetComponent<ButtonEffect>();
                navI.SpaceObject = objects[i].GetComponent<GalaxyObject>();
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
}
