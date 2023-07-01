using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Core.Inject.FoldersManagerService;
using Core.Core.Inject.GlobalDataService;
using Core.Dialogs;
using Core.Dialogs.Visuals;
using Core.Game;
using Core.Location;
using Core.PlayerScripts;
using Core.Systems;
using Core.UI;
using UnityEngine;
using Zenject;
using Random = System.Random;


public class PlayerTutorial : Singleton<PlayerTutorial>
{
    private TutorialsManager.Tutorial tutorial;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject questPoint;
    [SerializeField] private Dialog m1, m2, m3, m3_2;
    [SerializeField] private ItemShip knight;
    [SerializeField] private Item transmitterItem;
    [SerializeField] private Item zincItem;


    private GameObject station;

    [Inject]
    public void Constructor(SolarSystemService solarSystemService, FolderManagerService folderManagerService)
    {
        Single(this);
        
        this.solarSystemService = solarSystemService;
        tutorial = TutorialsManager.LoadTutorial(folderManagerService);
        Init();
    }
    
    public void Init()
    {
        var activator = FindObjectOfType<LocationUIActivator>();
        Player.inst.quests.CancelQuest(Player.inst.quests.quests.Find(x => x.questID == int.MaxValue));
        if (tutorial.CommunitsBaseStats == null)
        {
            if (!tutorial.m1_Dialog1)
            {
                CreateMessenger(activator.transform, m1, () =>
                {
                    tutorial.startSystemName = solarSystemService.CurrentSolarSystem.name;
                });
            }
            else if (tutorial.startSystemName != "")
            {
                M1GenerateQuest(false);
            }
        }
        else if (tutorial.CommunitsBaseStats.isSeeDemo && !tutorial.seeTranslatorDemo && !tutorial.m3_Dialog3)
        {
            if (!tutorial.m2_Dialog2)
            {
                CreateMessenger(activator.transform, m2, null);
            }
            else
            {
                M2GenerateQuest(true);
                M2Events();
            }
            AddStation();
        }
        else if (tutorial.CommunitsBaseStats.isSeeDemo && tutorial.seeTranslatorDemo)
        {
            if (!tutorial.m3_Dialog3)
            {
                CreateMessenger(activator.transform, m3, null);
            }
            else
            {
                M3GenerateQuest(true);
               
            }
            AddStation();
        }
    }

    public void CreateMessenger(Transform activator, Dialog dialog, Action actionBeforeSave)
    {
        var messenger = Instantiate(dialogue, activator.transform.position, activator.transform.rotation, activator.transform.parent).GetComponent<DialogMessenger>();
        messenger.dialog = dialog;
        messenger.Init();
        actionBeforeSave?.Invoke();
        TutorialsManager.SaveTutorial(tutorial, folderManagerService);
        EnablePlayer(false);
    }
    
    #region M1
    
    public void M1AddStation()
    {
        if (station != null)
        {
            Destroy(station.gameObject);
        }
        var quest = Player.inst.quests.quests.Find(x => x.questID == int.MaxValue);
        if ((quest != null && quest.GetLastQuestPath().solarName == solarSystemService.CurrentSolarSystem.name) ||
            tutorial.baseSystemName == solarSystemService.CurrentSolarSystem.name)
        {
            var rnd = new System.Random(NamesHolder.StringToSeed(tutorial.startSystemName));
            var point = Instantiate(questPoint, Vector3.zero, Quaternion.identity, FindObjectOfType<SpaceManager>().transform);
            var pos = new Vector3(rnd.Next(5000, 10000) * (rnd.Next(-5, 5) <= 0 ? 1 : -1) , rnd.Next(1000, 2000) * (rnd.Next(-5, 5) <= 0 ? 1 : -1), rnd.Next(5000, 10000) * (rnd.Next(-5, 5) <= 0 ? 1 : -1));
            point.transform.localPosition = pos;
            point.GetComponent<ContactObject>().Init();
            point.name = "Communist Space Unorbital Station";
            station = point;
            tutorial.baseSystemName = solarSystemService.CurrentSolarSystem.name;
            TutorialsManager.SaveTutorial(tutorial, folderManagerService);
            StartCoroutine(M1AddStationUpdate());
        }
    }

    IEnumerator M1AddStationUpdate()
    {
        yield return null;
        Player.OnSceneChanged -= M1AddStation;
        Player.OnSceneChanged.Run();
        Player.OnSceneChanged += M1AddStation;
        Player.inst.targets.ContactsChanges.Run();
    }
    
    public void M1Quest()
    {
        M1GenerateQuest(true);
        tutorial.m1_Dialog1 = true;
        TutorialsManager.SaveTutorial(tutorial, folderManagerService);
    }
    
    private void M1GenerateQuest(bool notify)
    {
        Player.inst.quests.CancelQuest(Player.inst.quests.quests.Find(x => x.questID == int.MaxValue));
        
        var quest = GetEmptyQuest();
        quest.keyValues.Add("Text", "Transfer to the system with the base, then select it in the Navigation Tab, and jump into it.");
        quest.appliedSolar = tutorial.startSystemName;
        quest.appliedStation = "";
        quest.GetPath(new System.Random(int.MaxValue), "Communists Base", tutorial.startSystemName, 1, 3, false);
        quest.GetLastQuestPath().targetName = "Communists Base";
        quest.toTransfer = new List<Item>();
        Player.inst.quests.ApplyQuest(quest, notify);
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

    public void M2Quest()
    {
        M2GenerateQuest(true);
        tutorial.m2_Dialog2 = true;
        M2Events();
        TutorialsManager.SaveTutorial(tutorial, folderManagerService);
    }


    public void M2Events()
    {
        Player.inst.cargo.OnChangeInventory += HaveTranslator;
        HaveTranslator();
    }
    
    public void M3Events()
    {
        Player.inst.cargo.OnChangeInventory += HaveZincVoid;
        HaveZinc();
    }

    public void HaveTranslator()
    {
        if (Player.inst.cargo.ContainItem(transmitterItem.id.id))
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
        Player.inst.quests.ApplyQuest(quest, notify);
    }
    
    #endregion

    #region M3

    public void M3Quest()
    {
        M3GenerateQuest(true);
        tutorial.m3_Dialog3 = true;
        TutorialsManager.SaveTutorial(tutorial, folderManagerService);
    }


    private string getKnight = "Get on the ship \"Knight\".";
    private string getZinc = "Obtain one or more full zinc stores.";
    private string flyToStation = "Fly to the communist station and make your way to the rear";
    public void M3GenerateQuest(bool notify)
    {
        var quest = GetEmptyQuest();
        if (Player.inst.Ship().shipName != knight.shipName)
        {
            quest.keyValues.Add("Text", getKnight);
        }
        else
        {
            if (!tutorial.m3_Dialog4)
            {
                var activator = FindObjectOfType<LocationUIActivator>();
                CreateMessenger(activator.transform, m3_2, null);
                tutorial.m3_Dialog4 = true;
                TutorialsManager.SaveTutorial(tutorial, folderManagerService);
            }
            
            if (!HaveZinc())
            {
                quest.keyValues.Add("Text",getZinc);
            }
            else
            {
                quest.keyValues.Add("Text", flyToStation);
                Player.inst.cargo.OnChangeInventory -= HaveZincVoid;
            }
            M3Events();
        }
        Player.inst.quests.ApplyQuest(quest, notify);
    }

    private bool haveZinc;
    private FolderManagerService folderManagerService;
    private SolarSystemService solarSystemService;

    public bool HaveZinc()
    {
        var contain = Player.inst.cargo.ContainItem(zincItem.id.id);
        var quest = Player.inst.quests.quests.Find(x => x.questID == int.MaxValue);
        if (quest != null)
        {
            if (contain)
            {
                var isFull = Player.inst.cargo.FindItem(zincItem.id.id).amount.IsFull();
                if (isFull != haveZinc)
                {
                    quest.keyValues["Text"] = flyToStation;
                    Player.inst.quests.OnChangeQuests.Run();
                    haveZinc = true;
                }

                return isFull;
            }
            else if (haveZinc)
            {
                quest.keyValues["Text"] = getZinc;
                Player.inst.quests.OnChangeQuests.Run();
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
        Player.inst.land.enabled = enable;
        Player.inst.control.enabled = enable;
        WorldSpaceObjectCanvas.Instance.gameObject.SetActive(enable);

    }
}
