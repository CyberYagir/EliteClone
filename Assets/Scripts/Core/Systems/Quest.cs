﻿using System;
using System.Collections;
using System.Collections.Generic;
using Core.Galaxy;
using Core.Game;
using Core.Location;
using Core.Player;
using Core.Systems;
using Core.UI;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    [System.Serializable]
    public class Quest
    {

        public static List<string> QuestTypes = new List<string>() {"Kill", "Mine", "Transfer"};

        public static string GetNameByID(int id)
        {
            return QuestTypes[id];
        }
        
        
        public enum QuestComplited
        {
            None, BrokeQuest, Complited, Rewarded
        }
        
        public Character quester;
        public int questType;
        public int questID;
        public QuestPath pathToTarget = new QuestPath();
        public QuestComplited questState;

        public Reward reward = new Reward();
        public string appliedStation, appliedSolar;
        public List<Item> toTransfer = new List<Item>();
        public string buttonText;

        public Quest(int questSeed , Character character, string stationName, string appliedSolar)
        {
            appliedStation = stationName;
            this.appliedSolar = appliedSolar;
            
            Init(questSeed, character);
        }

        public Quest()
        {

        }

        public bool IsTypeQuest(string str)
        {
            return questType == QuestTypes.FindIndex(x => String.Equals(x.Trim(), str.Trim(), StringComparison.CurrentCultureIgnoreCase));
        }
        
        public QuestPath GetLastQuestPath()
        {
            var last = pathToTarget;
            while (!last.isLast)
            {
                last = last.nextPath;
            }

            return last;
        }

        public List<string> ConvertToStrings()
        {
            List<string> names = new List<string>();
            var last = pathToTarget;
            names.Add(last.solarName);
            while (!last.isLast)
            {
                last = last.nextPath;
                names.Add(last.solarName);
            }

            return names;
        }

        public int JumpsCount()
        {
            int count = 0;
            var last = pathToTarget;
            while (!last.isLast)
            {
                last = last.nextPath;
                count++;
            }

            return count;
        }

        public void Init(int questSeed, Character character)
        {
            var rnd = new Random(questSeed);
            quester = character;
            questType = rnd.Next(0, QuestTypes.Count);
            questID = questSeed;

            if (WorldStationQuests.Instance != null)
            {
                WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.Init);
            }
            else
            {
                Player.Player.inst.StartCoroutine(Wait());
            }
        }

        IEnumerator Wait()
        {
            while (WorldStationQuests.Instance == null)
            {
                yield return null;
                WorldStationQuests.SetInstance();
            }
            WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.Init);
            Player.Player.inst.quests.OnChangeQuests.Run();
        }
        
        public void CheckIsQuestCompleted()
        {
            WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.IsCompleteCheck);
        }

        public QuestPath GetPath(Random rnd, string stationName, string solarName)
        {
            int pathLength = rnd.Next(0, 7);
            List<string> pathNames = new List<string>();
            QuestPath last = new QuestPath() {solarName = solarName};
            QuestPath first = last;
            pathNames.Add(last.solarName);
            pathToTarget = last;
            int trys = 0;
            bool stopPath = false;
            for (int i = 0; i < pathLength; i++)
            {
                if (GalaxyGenerator.systems[last.solarName].sibligs.Count != 0)
                {
                    var sibling = GalaxyGenerator.systems[last.solarName].sibligs[rnd.Next(0, GalaxyGenerator.systems[last.solarName].sibligs.Count)];
                    trys = 0;
                    while (pathNames.Contains(sibling.solarName) || GalaxyGenerator.systems[last.solarName].stations.Count == 0)
                    {
                        sibling = GalaxyGenerator.systems[last.solarName].sibligs[rnd.Next(0, GalaxyGenerator.systems[last.solarName].sibligs.Count)];
                        trys++;
                        if (trys > 5)
                        {
                            stopPath = true;
                            break;
                        }
                    }

                    if (stopPath) break;

                    pathNames.Add(sibling.solarName);
                    var newPath = new QuestPath() {prevPath = last, solarName = sibling.solarName};
                    last.nextPath = newPath;
                    last = newPath;
                }
                else
                {
                    break;
                }
            }

            var lastSolar = GalaxyGenerator.systems[last.solarName];

            if (lastSolar.stations.Count != 0)
            {
                last.targetName = lastSolar.stations[rnd.Next(0, lastSolar.stations.Count)].name;
                trys = 0;
                while (last.targetName == stationName)
                {
                    last.targetName = lastSolar.stations[rnd.Next(0, lastSolar.stations.Count)].name;
                    trys++;
                    if (trys > 5)
                    {
                        break;
                    }
                }
            }
            else
            {
                questState = QuestComplited.BrokeQuest;
            }

            return first;
        }

        public void GetButtonText()
        {
            WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.ButtonDisplay);
        }

        public bool FinishQuest()
        {
            if (Player.Player.inst.cargo.ContainItems(toTransfer))
            {
                Player.Player.inst.cargo.RemoveItems(toTransfer);
                if (Player.Player.inst.cargo.AddItems(reward.rewardItems))
                {
                    if (AppliedQuests.Instance.FinishQuest(questID))
                    {
                        questState = QuestComplited.Rewarded;
                        return true;
                    }
                }
                else
                {
                    Player.Player.inst.cargo.AddItems(toTransfer);
                    return false;
                }
            }

            return false;
        }
    }
}