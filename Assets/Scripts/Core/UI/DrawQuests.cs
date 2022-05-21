using System;
using Core.Location;
using Core.PlayerScripts;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class DrawQuests : MonoBehaviour
    {
        [SerializeField] private RectTransform holder, item;
        [SerializeField] private UpDownUI upDownUI;
        [SerializeField] private GameObject notify;
        private AppliedQuests quests;
        private float height;
        private void Awake()
        {
            quests = Player.inst.GetComponent<AppliedQuests>();
            quests.OnChangeQuests += OnChangeQuests;
            quests.OnNotify += Notify;
            Player.OnSceneChanged += OnChangeQuests;
            height = item.sizeDelta.y;
        }

        public void Notify()
        {
            notify.transform.localScale = Vector3.zero;
            notify.transform.DOScale(Vector3.one, 0.2f);
            notify.SetActive(true);
        }
        private void Update()
        {
            if (upDownUI.selectedIndex != -1)
            {
                holder.anchoredPosition = Vector2.Lerp(holder.anchoredPosition, new Vector2(0, height * upDownUI.selectedIndex), 10 * Time.deltaTime);
            }
        }

        private void OnEnable()
        {
            notify.SetActive(false);
        }

        private void OnChangeQuests()
        {
            UITweaks.ClearHolder(holder);
            int count = 0;
            for (int i = 0; i < quests.quests.Count; i++)
            {
                if (quests.quests[i].questState == Quest.QuestCompleted.None || quests.quests[i].questState == Quest.QuestCompleted.Completed)
                {
                    var newItem = Instantiate(item, holder);
                    var q = newItem.GetComponent<QuestTabItem>();
                    q.Init(quests.quests[i]);
                    newItem.gameObject.SetActive(true);
                    count++;
                }
            }
            upDownUI.itemsCount = count;
            LayoutRebuilder.ForceRebuildLayoutImmediate(holder.GetComponent<RectTransform>());
        }
    }
}
