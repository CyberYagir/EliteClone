using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class DrawQuests : MonoBehaviour
{
    [SerializeField] private Transform holder, item;
    private AppliedQuests quests;
    private void Awake()
    {
        quests = Player.inst.GetComponent<AppliedQuests>();
        quests.OnChangeQuests += OnChangeQuests;
    }

    private void OnChangeQuests()
    {
        UITweaks.ClearHolder(holder);
 
        for (int i = 0; i < quests.quests.Count; i++)
        {
            var newItem = Instantiate(item, holder);
            var q = newItem.GetComponent<QuestTabItem>();
            q.Init(quests.quests[i]);
            newItem.gameObject.SetActive(true);
        }
    }
}
