using System;
using System.Collections;
using System.Collections.Generic;
using Core.Galaxy;
using Core.Game;
using Core.Map;
using Core.PlayerScripts;
using Core.Quests;
using Core.Systems;
using Newtonsoft.Json;
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
        
        
        public string targetSolar { get; set; }
        public string targetStructure { get; set; }
        
        public List<Item> toTransfer { get; set; } = new List<Item>();
        public Reward reward { get; private set; } = new Reward();

        public List<string> CurrentPath => currentPath;

        public bool isOnLoading = false;
        
        private List<string> currentPath = new List<string>();

        public Dictionary<string, object> keyValues = new Dictionary<string, object>();
        public QuestCompleted questState;
        public string buttonText;




        public Quest(int questSeed , Character character, string stationName, string appliedSolar, QuestsMethodObject methods)
        {
            this.appliedStation = stationName;
            this.appliedSolar = appliedSolar;
            
            Init(questSeed, character, methods);
        }

        public Quest(){}

        public bool IsTypeQuest(string str) => questType == WorldDataItem.Quests.NameToID(str);
        

        public int JumpsCount()
        {
            return CurrentPath.Count;
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
                PlayerDataManager.Instance.WorldHandler.ShipPlayer.AppliedQuests.OnChangeQuests.Run();
            }
        }
        
        public void CheckIsQuestCompleted()
        {
            WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.IsCompleteCheck);
        }

        public QuestPath GenerateRandomPath(Random rnd, string stationName, string solarName, int minJumps = 4)
        {
            List<List<string>> paths = new List<List<string>>();
            List<string> targets = new List<string>();
            foreach (var pathStart in GalaxyGenerator.systems[solarName].sibligs)
            {
                var path = new List<string>();
                path.Add(pathStart.solarName);
                var targetSystem = GalaxyGenerator.systems[pathStart.solarName];
                var targetStation = "";
                while (path.Count < minJumps || targetSystem.stations.Count == 0)
                {
                    if (targetSystem.sibligs.Count == 0) break;
                    var next = targetSystem.sibligs[rnd.Next(0, targetSystem.sibligs.Count)].solarName;

                    path.Add(next);
                    targetSystem = GalaxyGenerator.systems[next];

                    if (targetSystem.stations.Count != 0)
                    {
                        targetStation = targetSystem.stations[rnd.Next(0, targetSystem.stations.Count)].name;
                    }
                }

                paths.Add(path);
                targets.Add(targetStation);

                if (path.Count > minJumps && !string.IsNullOrEmpty(targetStation))
                {
                    break;
                }
            }

            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i].Count > minJumps && !string.IsNullOrEmpty(targets[i]))
                {
                    QuestPath start = new QuestPath()
                    {
                        solarName = solarName,
                        targetName = stationName,
                    };

                    var last = start;

                    for (int j = 0; j < paths[i].Count; j++)
                    {
                        var next = new QuestPath()
                        {
                            solarName = paths[i][j],
                            targetName = ""
                        };

                        last.nextPath = next;

                        last = next;
                    }

                    last.GetLastQuestPath().targetName = targets[i];
                    
                    return start;
                }
            }

            return null;
        }


        public void RegeneratePathFromPlayer()
        {
            if (currentPath.Contains(PlayerDataManager.Instance.WorldHandler.CurrentSolarSystem.name)) return;

            if (IsEmptyQuest()) return;

            isOnLoading = true;
            var path = new List<string>();
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.GalaxyFinder.FindPathFrom(path, PlayerDataManager.Instance.WorldHandler.CurrentSolarSystem, targetSolar, delegate(List<string> list)
            {
                currentPath = list;
                isOnLoading = false;
                PlayerDataManager.Instance.WorldHandler.ShipPlayer.AppliedQuests.OnChangeQuests.Run();
            });
        }

        public bool IsEmptyQuest()
        {
            return string.IsNullOrEmpty(appliedSolar) || string.IsNullOrEmpty(targetSolar);
        }

        public void GetButtonText()
        {
            WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.ButtonDisplay);
        }
        public void OnFinish()
        {
            WorldStationQuests.Instance.GetEventByID(questType)?.Execute(this, WorldStationQuests.QuestFunction.ExecuteType.isCompleted);
        }

        public void SetBasePath(QuestPath generateRandomPath)
        {
            if (generateRandomPath == null)
            {
                questState = QuestCompleted.BrokeQuest;
                return;
            }
            var lastNode = generateRandomPath.GetLastQuestPath();
            
            
            targetSolar = lastNode.solarName;
            targetStructure = lastNode.targetName;

            RegeneratePathFromPlayer();
        }
    }
}