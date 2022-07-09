using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Core.Dialogs;
using Core.Dialogs.Visuals;
using Core.Game;
using Core.Location;
using Core.PlayerScripts;
using Core.Systems;
using Core.UI;
using UnityEngine;
using Random = System.Random;


public class PlayerTutorial : Singleton<PlayerTutorial>
{
    private TutorialsManager.Tutorial tutorial;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject questPoint;
    private void Awake()
    {
        Single(this);
    }

    private void Start()
    {
        tutorial = TutorialsManager.LoadTutorial();
        M1Init();

    }

    #region M1

    public void M1Init()
    {
        var activator = FindObjectOfType<LocationUIActivator>();
        if (!tutorial.m1_Dialog1)
        {
            var messenger = Instantiate(dialogue, activator.transform.position, activator.transform.rotation, activator.transform.parent).GetComponent<DialogMessenger>();
            messenger.dialog = (Dialog)Resources.Load("Game/Dialogs/M1", typeof(Dialog));
            tutorial.startSystemName = PlayerDataManager.CurrentSolarSystem.name;
            TutorialsManager.SaveTutorial(tutorial);
            EnablePlayer(false);
        }
        else if (tutorial.startSystemName != "")
        {
            M1GenerateQuest(false);
        }
    }
    
    public void M1AddStation()
    {
        if (Player.inst.quests.quests.Find(x => x.questID == int.MaxValue).GetLastQuestPath().solarName == PlayerDataManager.CurrentSolarSystem.name)
        {
            var rnd = new System.Random(NamesHolder.StringToSeed(tutorial.startSystemName));
            var point = Instantiate(questPoint, Vector3.zero, Quaternion.identity, FindObjectOfType<SpaceManager>().transform);
            var pos = new Vector3(rnd.Next(5000, 10000) * (rnd.Next(-5, 5) <= 0 ? 1 : -1) , rnd.Next(1000, 2000) * (rnd.Next(-5, 5) <= 0 ? 1 : -1), rnd.Next(5000, 10000) * (rnd.Next(-5, 5) <= 0 ? 1 : -1));
            point.transform.localPosition = pos;
            point.GetComponent<ContactObject>().Init();
            point.name = "Communist Space Unorbital Station";
            tutorial.baseSystemName = PlayerDataManager.CurrentSolarSystem.name;
            TutorialsManager.SaveTutorial(tutorial);
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
        TutorialsManager.SaveTutorial(tutorial);
    }

    public void M1GenerateQuest(bool notify)
    {
        Player.inst.quests.CancelQuest(Player.inst.quests.quests.Find(x => x.questID == int.MaxValue));
        Quest quest = new Quest();
        Character character = new Character();
        character.fraction = WorldDataItem.Fractions.NameToID("Libertarians");
        character.firstName = "Khatuna";
        character.lastName = "Tupaq";
        character.characterID = 0;
        quest.Init(0, character, true);
        quest.questType = -1;
        quest.questID = int.MaxValue;
        quest.keyValues.Add("Text", "Transfer to the system with the base, then select it in the Navigation Tab, and jump into it.");
        quest.appliedSolar = tutorial.startSystemName;
        quest.appliedStation = "";
        quest.GetPath(new System.Random(int.MaxValue), "Communists Base", tutorial.startSystemName, 1, 3, false);
        quest.GetLastQuestPath().targetName = "Communists Base";
        quest.toTransfer = new List<Item>();
        Player.inst.quests.ApplyQuest(quest, notify);
        Player.OnSceneChanged += M1AddStation;
        M1AddStation();
    }
    #endregion

    

    public static void EnablePlayer(bool enable)
    {
        Player.inst.land.enabled = enable;
        Player.inst.control.enabled = enable;
        WorldSpaceObjectCanvas.Instance.gameObject.SetActive(enable);

    }
}
