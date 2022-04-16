using Core.Location;
using Core.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
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
            if (upDownUI.selectedIndex != -1)
            {
                holder.anchoredPosition = Vector2.Lerp(holder.anchoredPosition, new Vector2(0, height * upDownUI.selectedIndex), 10 * Time.deltaTime);
            }
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
