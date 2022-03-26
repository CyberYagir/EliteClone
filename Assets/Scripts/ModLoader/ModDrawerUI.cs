using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UI;
using UnityEngine;

public class ModDrawerUI : MonoBehaviour
{
    [SerializeField] private Transform item, holder;

    public void Start()
    {
        Redraw();
    }

    public void Redraw()
    {
        item.gameObject.SetActive(false);
        UITweaks.ClearHolder(holder.transform);
        var mods = ModsManager.Instance.modLoader.mods;
        for (int i = 0; i < ModsManager.Instance.modLoader.loadChain.mods.Count; i++)
        {
            var modItem = Instantiate(item, holder).GetComponent<ModItemUI>();
            modItem.Init(mods.Find(x=>x.data.modName == Path.GetFileNameWithoutExtension(ModsManager.Instance.modLoader.loadChain.mods[i])));
            modItem.gameObject.SetActive(true);
        }
    }

    public void ReloadApp()
    {
        ModsManager.Instance.ReloadMods();
    }
}
