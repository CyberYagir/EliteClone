using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using UI;
using UnityEngine;

public class DrawQuests : MonoBehaviour
{
    [SerializeField] private RectTransform holder, item;
    [SerializeField] private UpDownUI upDownUI;
    private AppliedQuests quests;
    private float height;
    private void Awake()
    {
        quests = Player.inst.GetComponent<AppliedQuests>();
        quests.OnChangeQuests += OnChangeQuests;
        Player.OnSceneChanged += OnChangeQuests;
        height = item.sizeDelta.y;
    }

    private void Update()
    {
        holder.anchoredPosition = Vector2.Lerp(holder.anchoredPosition, new Vector2(0, height * upDownUI.selectedIndex), 10 * Time.deltaTime);
    }

    private void OnChangeQuests()
    {
        UITweaks.ClearHolder(holder);
        int count = 0;
        for (int i = 0; i < quests.quests.Count; i++)
        {
            if (quests.quests[i].questState != Quest.QuestComplited.Rewarded)
            {
                var newItem = Instantiate(item, holder);
                var q = newItem.GetComponent<QuestTabItem>();
                q.Init(quests.quests[i]);
                newItem.gameObject.SetActive(true);
                count++;
            }
        }
        upDownUI.itemsCount = count;
    }
}
