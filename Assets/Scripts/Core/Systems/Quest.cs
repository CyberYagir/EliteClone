using System;
using System.Collections;
using System.Collections.Generic;
using Core.Galaxy;
using Core.Game;
using Core.PlayerScripts;
using Core.Quests;
using Core.Systems;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    [Serializable]
    public class Quest
    {
        public enum QuestCompleted
        {
            None, BrokeQuest, Completed, Rewarded
        }
        
        
        public Character quester { get; private set; }
        public int questType { get; set; }
        public int questID { get; set; }

        public int questCost { get; private set; }

        public string appliedStation { get; set; }
        public string appliedSolar { get; set; }
        public List<Item> toTransfer { get; set; } = new List<Item>();
        public Reward reward { get; private set; } = new Reward();
        
        
        public Dictionary<string, object> keyValues = new Dictionary<string, object>();
        public QuestPath pathToTarget = new QuestPath();
        public QuestCompleted questState;
        public string buttonText;




        public Quest(int questSeed , Character character, string stationName, string appliedSolar, QuestsMethodObject methods)
        {
            appliedStation = stationName;
            this.appliedSolar = appliedSolar;
            Init(questSeed, character, methods);
        }

        public Quest(){}

        public bool IsTypeQuest(string str) => questType == WorldDataItem.Quests.NameToID(str);
        
        public QuestPath GetLastQuestPath()
        {
            var last = pathToTarget;
            if (last != null)
            {
                while (!last.isLast)
                {
                    last = last.nextPath;
                }
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

        public void Init(int questSeed, Character character, QuestsMethodObject methdos, bool notStation = false)
        {
            var rnd = new Random(questSeed);
            quester = character;
            questType = rnd.Next(0, WorldDataItem.Quests.Count);
            questID = questSeed;
            questCost = rnd.Next(1, 5);
            if (!notStation)
            {
                if (methdos != null)
                {
                    methdos.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.Init);
                }
            }
        }

        IEnumerator Wait()
        {
            if (World.Scene == Scenes.Location)
            {
                int framesCount = 0;
                while (WorldStationQuests.Instance == null)
                {
                    yield return null;
                    WorldStationQuests.SetInstance();
                    framesCount++;
                }

                WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.Init);
                PlayerDataManager.Instance.WorldHandler.ShipPlayer.quests.OnChangeQuests.Run();
            }
        }
        
        public void CheckIsQuestCompleted()
        {
            WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.IsCompleteCheck);
        }

        public QuestPath GetPath(Random rnd, string stationName, string solarName, int minJumps = 0, int maxJumps = 7, bool canBroke = true)
        {
            int pathLength = rnd.Next(minJumps, maxJumps);
            List<string> pathNames = new List<string>();
            QuestPath last = new QuestPath {solarName = solarName};
            QuestPath first = last;
            pathNames.Add(last.solarName);
            pathToTarget = last;
            int trys = 0;
            bool stopPath = false;
            if (pathLength == 0) return last;
            for (int i = 0; i < pathLength; i++)
            {
                if (last.solarName != null){
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
                        var newPath = new QuestPath {prevPath = last, solarName = sibling.solarName};
                        last.nextPath = newPath;
                        last = newPath;
                    }
                }
                else
                {
                    break;
                }
            }
            if (last.solarName == null)
            {
                return last.prevPath;
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
            else if (canBroke)
            {
                questState = QuestCompleted.BrokeQuest;
            }

            return first;
        }

        public void GetButtonText()
        {
            WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.ButtonDisplay);
        }
        public void OnFinish()
        {
            WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.isCompleted);
        }
    }
}