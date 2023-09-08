using System;
using System.Collections;
using System.Collections.Generic;
using Core.Dialogs;
using Core.Dialogs.Visuals;
using Core.Game;
using Core.Garage;
using Core.Location;
using Core.PlayerScripts;
using Core.Systems;
using Core.UI;
using UnityEngine;


namespace Core.Core.Tutorial
{
    public class PlayerTutorial : Singleton<PlayerTutorial>
    {
        [SerializeField] private GameObject dialogue;
        [SerializeField] private GameObject questPoint;
        [SerializeField] private Dialog m1, m2, m3, m3_2, m3_3;
        [SerializeField] private ItemShip knight;
        [SerializeField] private Item transmitterItem;
        [SerializeField] private Item zincItem;
        [SerializeField] private FaderMultiScenes fader;


        private GameObject station;

        private TutorialsManager tutorialsManager;
        private WorldStructureService worldStructureManager;
        private WorldDataHandler worldDataHandler;
        
        
        private TutorialSO TutorialData => tutorialsManager.TutorialData;

        
        public void Init()
        {
            Single(this);
            
            tutorialsManager = PlayerDataManager.Instance.Services.TutorialsManager;
            worldStructureManager = PlayerDataManager.Instance.Services.WorldStructuresManager;
            worldDataHandler = PlayerDataManager.Instance.WorldHandler;
            
            
            activator = FindObjectOfType<LocationUIActivator>();
            CheckTutorialQuests(activator);
        }

        private void CheckTutorialQuests(LocationUIActivator activator)
        {
            var questID = TutorialData.TutorialQuestsData.QuestID;
            if (questID == 0)
            {
                CreateMessenger(activator.transform, m1,
                    null,
                    () =>
                    {
                        M1GenerateQuestStructures();
                        M1GenerateQuest(true);
                        TutorialData.TutorialQuestsData.NextTutorialQuest();
                    });
            }

            if (questID == 1)
            {
                M1GenerateQuest(false);
            }

            if (questID == 2)
            {
                CreateMessenger(activator.transform, m2,
                    null,
                    () =>
                    {
                        CancelBaseQuests();
                        M2GenerateQuest(true);
                        TutorialData.TutorialQuestsData.NextTutorialQuest();
                    });
            }

            if (questID == 3)
            {
                M2GenerateQuest(false);
            }

            if (questID == 4)
            {
                CreateMessenger(activator.transform, m3,
                    null,
                    () =>
                    {
                        M3GenerateQuest(true);
                        TutorialData.TutorialQuestsData.NextTutorialQuest();
                    });
            }

            if (questID == 5)
            {
                M3GenerateQuest(false);
            }

            if (questID == 6)
            {
                CreateMessenger(activator.transform, m3_2,
                    null,
                    () =>
                    {
                        M3GenerateQuest(true);
                        TutorialData.TutorialQuestsData.NextTutorialQuest();
                    });
            }

            if (questID == 7)
            {
                M3GenerateQuest(false);
            }

            if (questID == 8)
            {
                CreateMessenger(activator.transform, m3_2,
                    null,
                    () =>
                    {
                        M4GenerateQuest(true);
                        TutorialData.TutorialQuestsData.NextTutorialQuest();
                    });
            }
            if (questID == 9)
            {
                M4GenerateQuest(false);
            }

            if (questID == 10)
            {
                CreateMessenger(activator.transform, m3_3,
                    null,
                    () =>
                    {
                        M4GenerateQuest(true);
                        TutorialData.TutorialQuestsData.NextTutorialQuest();
                    });
            }

            if (questID == 11)
            {
                M4GenerateQuest(false);
            }
        }



        public void CancelBaseQuests()
        {
            var mainQuests = worldDataHandler.ShipPlayer.AppliedQuests.quests.FindAll(x => x.questID == int.MaxValue);
            foreach (var q in mainQuests)
            {
                worldDataHandler.ShipPlayer.AppliedQuests.CancelQuest(q);
            }
        }

        private void AddStationStructure(string solarName)
        {
            var rnd = new System.Random(NamesHolder.StringToSeed(solarName));
            var pos = new Vector3(rnd.Next(300, 500) * RandomSign(), rnd.Next(100, 300) * RandomSign(), rnd.Next(300, 500) * RandomSign());
            
            worldStructureManager.AddStructure("Communist Space Unorbital Station", solarName, StructureNames.ComunistsBase, pos);


            float RandomSign()
            {
               return (rnd.Next(-5, 5) <= 0 ? 1 : -1);
            }
        }
        
        public void CreateMessenger(Transform activator, Dialog dialog, Action beforeOpen, Action onClose)
        {
            var messenger = Instantiate(dialogue, activator.transform.position, activator.transform.rotation, activator.transform.parent).GetComponent<DialogMessenger>();
            messenger.dialog = dialog;
            beforeOpen?.Invoke();
            messenger.Init(onClose);
            tutorialsManager.Save();
            EnablePlayer(false);
        }
    
        #region M1


        public Quest M1GenerateQuest()
        {
            var quest = GetEmptyQuest();
            quest.appliedSolar = worldDataHandler.CurrentSolarSystem.name;
            var path = quest.GenerateRandomPath(new System.Random(int.MaxValue), "Communists Base", quest.appliedSolar, 2);
            
            quest.targetSolar = path.GetLastQuestPath().solarName;
            quest.targetStructure = "Communists Base";
            
            
            
            quest.keyValues.Add("Text", "Transfer to the system with the base, then select it in the Navigation Tab, and jump into it.");
            return quest;
        }
        
        private void M1GenerateQuestStructures()
        {
            var quest = M1GenerateQuest();
            
            AddStationStructure(quest.targetSolar);
            
            TutorialData.MainBaseData.SetBase();
            tutorialsManager.Save();
        }
        private void M1GenerateQuest(bool notify)
        {
            var (system, structure) = PlayerDataManager.Instance.Services.WorldStructuresManager.GetStructure(StructureNames.ComunistsBase);
            // var activeQuest = worldDataHandler.ShipPlayer.AppliedQuests.quests.Find(x => x.questID == int.MaxValue);

            CancelBaseQuests();
            
            if (structure == null)
            {
                var quest = M1GenerateQuest();
                worldDataHandler.ShipPlayer.AppliedQuests.ApplyQuest(quest, notify);
            }
            else
            {
                var newQuest = M1GenerateQuest();


                newQuest.targetStructure = "Communists Base";
                newQuest.targetSolar = system.SystemName;


                worldDataHandler.ShipPlayer.AppliedQuests.ApplyQuest(newQuest, false);
                print("CHANGE EXISTING QUEST");
            }
        }
        
        #endregion

        #region M2
        


        public void M2Events()
        {
            worldDataHandler.ShipPlayer.Cargo.OnChangeInventory -= HaveTranslator;
            worldDataHandler.ShipPlayer.Cargo.OnChangeInventory += HaveTranslator;
            HaveTranslator();
        }
    
        public void M3Events()
        {
            worldDataHandler.ShipPlayer.Cargo.OnChangeInventory -= HaveZincVoid;
            worldDataHandler.ShipPlayer.Cargo.OnChangeInventory += HaveZincVoid;
            if (HaveZinc())
            {
                NextDialog();
            }
        }

        public void HaveTranslator()
        {
            if (worldDataHandler.ShipPlayer.Cargo.ContainItem(transmitterItem.id.id))
            {
                PlayerPrefs.SetInt("last_level", (int) World.Scene);
                Instantiate(fader).LoadScene(Scenes.ActivatorDemo);
            }
        }

        public Quest GetEmptyQuest()
        {
            Quest quest = new Quest();
            Character character = new Character();
            character.fraction = WorldDataItem.Fractions.NameToID("Libertarians");
            character.firstName = "Khatuna";
            character.lastName = "Tupaq";
            character.characterID = 0;
            quest.Init(0, character, null, true);
            quest.questType = -1;
            quest.questID = int.MaxValue;
            quest.toTransfer = new List<Item>();

            return quest;
        }
        public void M2GenerateQuest(bool notify)
        {
            CancelBaseQuests();

            var weapons = GetComponent<Ship>().GetShip().slots.FindAll(x => x.slotType == ItemType.Weapon);

            var bindedWeapon = weapons.Find(x => x.button != -1);

            if (bindedWeapon != null)
            {
                M2Events();
            }
            
            var quest = GetEmptyQuest();
            quest.keyValues.Add("Text", "Activate the ship's weapons. And destroy the pirate's spaceship. Find the tachyon transmitter in the wreckage.");
            worldDataHandler.ShipPlayer.AppliedQuests.ApplyQuest(quest, notify);
        }
    
        #endregion

        #region M3
        

        private string getKnight = "Get on the ship \"Knight\".";
        private string getZinc = "Obtain one or more full zinc stores.";

        public void M3GenerateQuest(bool notify)
        {
            CancelBaseQuests();
            
            var quest = GetEmptyQuest();
            if (PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().shipName != knight.shipName)
            {
                quest.keyValues.Add("Text", getKnight);
            }
            else
            {
                NextDialog();
            }

            PlayerDataManager.Instance.WorldHandler.ShipPlayer.AppliedQuests.ApplyQuest(quest, notify);
        }

        private void NextDialog()
        {
            var activator = FindObjectOfType<LocationUIActivator>();
            TutorialData.TutorialQuestsData.NextTutorialQuest();
            CheckTutorialQuests(activator);
        }

        private bool haveZinc;
        private LocationUIActivator activator;

        public bool HaveZinc()
        {
            var contain = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Cargo.ContainItem(zincItem.id.id);
            var quest = PlayerDataManager.Instance.WorldHandler.ShipPlayer.AppliedQuests.quests.Find(x => x.questID == int.MaxValue);
            if (quest != null)
            {
                if (contain)
                {
                    var isFull = PlayerDataManager.Instance.WorldHandler.ShipPlayer.Cargo.FindItem(zincItem.id.id).amount.IsFull();
                    if (isFull != haveZinc)
                    {
                        PlayerDataManager.Instance.WorldHandler.ShipPlayer.AppliedQuests.OnChangeQuests.Run();
                        haveZinc = true;
                    }

                    return isFull;
                }
                
                if (haveZinc)
                {
                    quest.keyValues["Text"] = getZinc;
                    PlayerDataManager.Instance.WorldHandler.ShipPlayer.AppliedQuests.OnChangeQuests.Run();
                }
            }
            else
            {
                return contain;
            }

            haveZinc = false;
            return false;
        }

        public void HaveZincVoid() => HaveZinc();
    
        #endregion
        
        private void M4GenerateQuest(bool notify)
        {
            CancelBaseQuests();
            
            var quest = GetEmptyQuest();


            worldDataHandler.ShipPlayer.AppliedQuests.ApplyQuest(quest, notify);
            
            M3Events();
        }
        
        
        public static void EnablePlayer(bool enable)
        {
            var player = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
            player.LandManager.enabled = enable;
            player.Control.enabled = enable;
            WorldSpaceObjectCanvas.Instance.gameObject.SetActive(enable);

        }
    }
}
