using System.Collections.Generic;
using Core.Game;
using Core.Location;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.UI
{
    public class CharacterList : BaseTabUI
    {
        [SerializeField] private Transform holder;
        [SerializeField] private QuestListUI questList;
        [SerializeField] private QuesterItemUI item;
        [SerializeField] private ButtonActiveControlUI buttonsUI;
    
        private List<ButtonEffect> items = new List<ButtonEffect>();
        private BaseWindow baseWindow;


        public Event ChangeSelect = new Event();



        public void RedrawQuests()
        {
            if (WorldDataHandler.CurrentLocationGenerator != null && WorldDataHandler.CurrentLocationGenerator.CurrentLocationData.OrbitStation)
            {
                if (items.Count != 0 && upDownUI.selectedIndex != -1 && items.Count > upDownUI.selectedIndex) //Чтобы не было ошибок upDownUI.selectedIndex
                {
                    var character = items[upDownUI.selectedIndex].GetComponent<QuesterItemUI>().GetCharacter();
                    var quests = WorldDataHandler.CurrentLocationGenerator.CurrentLocationData.OrbitStation.quests.FindAll(x => x.quester == character);
                    quests.RemoveAll(x => AppliedQuests.Instance.quests.Find(y => y.questState == Quest.QuestCompleted.Rewarded && y.questID == x.questID) != null);
                    questList.UpdateQuests(quests);
                }
            }
        }


        public override void Init()
        {
            base.Init();
            
            baseWindow = GetComponentInParent<BaseWindow>();
            WorldDataHandler.OnChangeLocation += UpdateList;
            upDownUI.OnChangeSelected += ChangeSelected;
            upDownUI.OnNavigateChange += ChangeSelected;
            ChangeSelect += RedrawQuests;
            WorldDataHandler.ShipPlayer.land.OnLand += Enable;
            
            if (upDownUI.selectedIndex == -1)
            {
                upDownUI.ForceChangeSelect(0);
            }
        }

        public void ChangeSelected()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].over = upDownUI.selectedIndex == i ? ButtonEffect.ActionType.Over : ButtonEffect.ActionType.None;
            }
            ChangeSelect.Run();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (WorldDataHandler.ShipPlayer.land.isLanded)
            {
                ChangeTab();
            }
        }

        public void ChangeTab()
        {
            if (InputService.GetAxisDown(KAction.TabsHorizontal))
            {
                if (InputService.GetAxisRaw(KAction.TabsHorizontal) > 0)
                {
                    if (questList.QuestsCount != 0)
                    {
                        questList.SkipFrame();
                        questList.ChangeSelected();
                        questList.Enable();
                        Disable();
                    }
                }

                if (InputService.GetAxisRaw(KAction.TabsHorizontal) < 0)
                {
                    buttonsUI.SkipFrame();
                    buttonsUI.enabled = true;
                    Disable();
                }
            }

            if (upDownUI.itemsCount != 0)
            {
                var rect = holder.GetComponent<RectTransform>();
                rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(0, upDownUI.selectedIndex * 255f), 10 * Time.deltaTime); //255 - Высота итема магическим числом так как лень
            }
        }

        public void UpdateList()
        {
            UITweaks.ClearHolder(holder);
        
            items = new List<ButtonEffect>();

            var orbitStation = WorldDataHandler.CurrentLocationGenerator.CurrentLocationData.OrbitStation;
            orbitStation.RemoveCharactersWithoutQuests();
            int id = 0;
            foreach (var character in orbitStation.characters)
            {
                var it = Instantiate(item.gameObject, holder).GetComponent<QuesterItemUI>();
                it.InitQuesterItem(character.fraction, WorldDataItem.Fractions.IconByID(character.fraction), character.firstName + " " + character.lastName, character, id);
                it.gameObject.SetActive(true);
                var bEffect = it.GetComponent<ButtonEffect>();
                items.Add(bEffect);
                if (orbitStation.additionalCharacters.Contains(character.characterID))
                {
                    bEffect.SetNoneColor(Color.green);
                }

                id++;
            }
            upDownUI.itemsCount = orbitStation.characters.Count;

            ChangeSelected();
        }
    }
}
