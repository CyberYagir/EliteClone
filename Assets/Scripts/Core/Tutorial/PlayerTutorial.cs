using System;
using System.Collections;
using System.Collections.Generic;
using Core.Dialogs;
using Core.Dialogs.Visuals;
using Core.Game;
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
        [SerializeField] private Dialog m1, m2, m3, m3_2;
        [SerializeField] private ItemShip knight;
        [SerializeField] private Item transmitterItem;
        [SerializeField] private Item zincItem;



        private GameObject station;

        private TutorialsManager tutorialsManager;
        private WorldDataHandler worldDataHandler;
        
        
        private TutorialSO TutorialData => tutorialsManager.TutorialData;

        public string CurrentTutorialQuestIsReaded => $"tutorQuest[{TutorialData.TutorialQuestsData.QuestID}-MessageRead";
        
        private void Awake()
        {
            Single(this);
            
            
            tutorialsManager = PlayerDataManager.Instance.Services.TutorialsManager;
            worldDataHandler = PlayerDataManager.Instance.WorldHandler;
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            var activator = FindObjectOfType<LocationUIActivator>();

            CheckTutorialQuests(activator);
        }

        private void CheckTutorialQuests(LocationUIActivator activator)
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.CancelQuest(PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.quests.Find(x => x.questID == int.MaxValue));
            var questID = TutorialData.TutorialQuestsData.QuestID;
            if (questID == 0)
            {
                CreateMessenger(activator.transform, m1,
                    () => TutorialData.MainBaseData.SetFistSystem(worldDataHandler.CurrentSolarSystem.name),
                    () =>
                    {
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
                        M2GenerateQuest(true);
                        M2Events();
                        TutorialData.TutorialQuestsData.NextTutorialQuest();
                    });
            }

            if (questID == 3)
            {
                M2GenerateQuest(false);
                M2Events();
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
                CreateMessenger(activator.transform, m3_2, null, null);
            }

            if (questID > 0)
            {
                AddStation();
            }
        }

        // private void OldQuestsSystem(LocationUIActivator activator)
        // {
        //     PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.CancelQuest(PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.quests.Find(x => x.questID == int.MaxValue));
        //     if (TutorialData.CommunitsBaseStats == null)
        //     {
        //         if (!TutorialData.m1_Dialog1)
        //         {
        //             CreateMessenger(activator.transform, m1, () => { TutorialData.startSystemName = worldDataHandler.CurrentSolarSystem.name; });
        //         }
        //         else if (TutorialData.startSystemName != "")
        //         {
        //             M1GenerateQuest(false);
        //         }
        //     }
        //     else if (TutorialData.CommunitsBaseStats.isSeeDemo && !TutorialData.seeTranslatorDemo && !TutorialData.m3_Dialog3)
        //     {
        //         if (!TutorialData.m2_Dialog2)
        //         {
        //             CreateMessenger(activator.transform, m2, null);
        //         }
        //         else
        //         {
        //             M2GenerateQuest(true);
        //             M2Events();
        //         }
        //
        //         AddStation();
        //     }
        //     else if (TutorialData.CommunitsBaseStats.isSeeDemo && TutorialData.seeTranslatorDemo)
        //     {
        //         if (!TutorialData.m3_Dialog3)
        //         {
        //             CreateMessenger(activator.transform, m3, null);
        //         }
        //         else
        //         {
        //             M3GenerateQuest(true);
        //         }
        //
        //         AddStation();
        //     }
        // }

        public void CreateMessenger(Transform activator, Dialog dialog, Action beforeOpen, Action onClose)
        {
            var messenger = Instantiate(dialogue, activator.transform.position, activator.transform.rotation, activator.transform.parent).GetComponent<DialogMessenger>();
            messenger.dialog = dialog;
            beforeOpen?.Invoke();
            messenger.Init(onClose);
            tutorialsManager.SaveTutorial();
            EnablePlayer(false);
        }
    
        #region M1

        public void M1AddStation()
        {
            if (station != null)
            {
                Destroy(station.gameObject);
            }

            var quest = PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.quests.Find(x => x.questID == int.MaxValue);
            if ((quest != null && quest.GetLastQuestPath().solarName == worldDataHandler.CurrentSolarSystem.name) ||
                TutorialData.MainBaseData.BaseSystemName == worldDataHandler.CurrentSolarSystem.name)
            {
                var rnd = new System.Random(NamesHolder.StringToSeed(TutorialData.MainBaseData.StartSystemName));
                var point = Instantiate(questPoint, Vector3.zero, Quaternion.identity, FindObjectOfType<SpaceManager>().transform);
                var pos = new Vector3(rnd.Next(5000, 10000) * (rnd.Next(-5, 5) <= 0 ? 1 : -1), rnd.Next(1000, 2000) * (rnd.Next(-5, 5) <= 0 ? 1 : -1), rnd.Next(5000, 10000) * (rnd.Next(-5, 5) <= 0 ? 1 : -1));
                point.transform.localPosition = pos;
                point.GetComponent<ContactObject>().Init();
                point.name = "Communist Space Unorbital Station";
                station = point;
                
                TutorialData.MainBaseData.SetBase(worldDataHandler.CurrentSolarSystem.name);
                
                tutorialsManager.SaveTutorial();
                StartCoroutine(M1AddStationUpdate());
            }
        }

        IEnumerator M1AddStationUpdate()
        {
            yield return null;
            Player.OnSceneChanged -= M1AddStation;
            Player.OnSceneChanged.Run();
            Player.OnSceneChanged += M1AddStation;
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.targets.ContactsChanges.Run();
        }
        

        private void M1GenerateQuest(bool notify)
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.CancelQuest(PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.quests.Find(x => x.questID == int.MaxValue));

            var quest = GetEmptyQuest();
            quest.keyValues.Add("Text", "Transfer to the system with the base, then select it in the Navigation Tab, and jump into it.");
            quest.appliedSolar = TutorialData.MainBaseData.StartSystemName;
            quest.appliedStation = "";
            quest.GetPath(new System.Random(int.MaxValue), "Communists Base", TutorialData.MainBaseData.StartSystemName, 2, 4, false);
            quest.GetLastQuestPath().targetName = "Communists Base";
            quest.toTransfer = new List<Item>();
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.ApplyQuest(quest, notify);
            AddStation();
        }

        public void AddStation()
        {
            Player.OnSceneChanged += M1AddStation;
            M1AddStation();
        }

        public void DestroyStation()
        {
            Destroy(station.gameObject);
        }
        #endregion

        #region M2
        


        public void M2Events()
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.OnChangeInventory += HaveTranslator;
            HaveTranslator();
        }
    
        public void M3Events()
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.OnChangeInventory += HaveZincVoid;
            HaveZinc();
        }

        public void HaveTranslator()
        {
            if (PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.ContainItem(transmitterItem.id.id))
            {
                PlayerPrefs.SetInt("last_level", (int) World.Scene);
                World.LoadLevel(Scenes.ActivatorDemo);
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
            var quest = GetEmptyQuest();
            quest.keyValues.Add("Text", "Activate the ship's weapons. And destroy the pirate's spaceship. Find the tachyon transmitter in the wreckage.");
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.ApplyQuest(quest, notify);
        }
    
        #endregion

        #region M3
        

        private string getKnight = "Get on the ship \"Knight\".";
        private string getZinc = "Obtain one or more full zinc stores.";
        private string flyToStation = "Fly to the communist station and make your way to the rear";
        public void M3GenerateQuest(bool notify)
        {
            var quest = GetEmptyQuest();
            if (PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().shipName != knight.shipName)
            {
                quest.keyValues.Add("Text", getKnight);
            }
            else
            {
                var activator = FindObjectOfType<LocationUIActivator>();
                
                
                TutorialData.TutorialQuestsData.NextTutorialQuest();
                CheckTutorialQuests(activator);
                

                if (!HaveZinc())
                {
                    quest.keyValues.Add("Text",getZinc);
                }
                else
                {
                    quest.keyValues.Add("Text", flyToStation);
                    PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.OnChangeInventory -= HaveZincVoid;
                }
                M3Events();
            }
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.ApplyQuest(quest, notify);
        }

        private bool haveZinc;
        public bool HaveZinc()
        {
            var contain = PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.ContainItem(zincItem.id.id);
            var quest = PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.quests.Find(x => x.questID == int.MaxValue);
            if (quest != null)
            {
                if (contain)
                {
                    var isFull = PlayerDataManager.Instance.WorldHandler.ShipPlayer.cargo.FindItem(zincItem.id.id).amount.IsFull();
                    if (isFull != haveZinc)
                    {
                        quest.keyValues["Text"] = flyToStation;
                        PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.OnChangeQuests.Run();
                        haveZinc = true;
                    }

                    return isFull;
                }
                else if (haveZinc)
                {
                    quest.keyValues["Text"] = getZinc;
                    PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.OnChangeQuests.Run();
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

        public static void EnablePlayer(bool enable)
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.land.enabled = enable;
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.control.enabled = enable;
            WorldSpaceObjectCanvas.Instance.gameObject.SetActive(enable);

        }
    }
}
