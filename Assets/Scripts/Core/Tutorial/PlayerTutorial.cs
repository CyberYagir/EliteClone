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


public class PlayerTutorial : Singleton<PlayerTutorial>
{
    private TutorialsManager.Tutorial tutorial;
    [SerializeField] private GameObject dialogue;

    private void Awake()
    {
        Single(this);
    }

    private void Start()
    {
        tutorial = TutorialsManager.LoadTutorial();

        var activator = FindObjectOfType<LocationUIActivator>();
        if (!tutorial.m1_Dialog1)
        {
            var messenger = Instantiate(dialogue, activator.transform.position, activator.transform.rotation, activator.transform.parent).GetComponent<DialogMessenger>();
            messenger.dialog = (Dialog)Resources.Load("Game/Dialogs/M1", typeof(Dialog));
        }

        EnablePlayer(false);
    }

    public void M1Quest()
    {
        Quest quest = new Quest();
        Character character = new Character();
        character.fraction = WorldDataItem.Fractions.NameToID("Libertarians");
        character.firstName = "Khatuna";
        character.lastName = "Tupaq";
        character.characterID = 0;
        quest.Init(0, character);
        quest.questType = -1;
        Player.inst.quests.ApplyQuest(quest);
    }

    
    public static void EnablePlayer(bool enable)
    {
        Player.inst.land.enabled = enable;
        Player.inst.control.enabled = enable;
        WorldSpaceObjectCanvas.Instance.gameObject.SetActive(enable);

    }
}
