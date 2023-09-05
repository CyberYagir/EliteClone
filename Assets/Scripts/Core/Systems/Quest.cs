using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Core;
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
            var startStation = GalaxyGenerator.systems[solarName];
            List<string> findedPath = null;
            for (int i = 0; i < startStation.sibligs.Count; i++)
            {
                var path = BuildPathRecursive(GalaxyGenerator.systems[startStation.sibligs[i].solarName], new List<string>(), minJumps, rnd);
                if (path != null)
                {
                    findedPath = path;
                    break;
                }
            }

            if (findedPath == null || findedPath.Count == 0)
            {
                return GenerateRandomPath(rnd, stationName, solarName, minJumps - 1);
            }



            QuestPath start = new QuestPath()
            {
                solarName = solarName,
                targetName = stationName,
            };
            var last = start;
            for (int j = 0; j < findedPath.Count; j++)
            {
                var next = new QuestPath()
                {
                    solarName = findedPath[j],
                    targetName = "",
                };
                last.nextPath = next;
                last = next;
            }

            var system = GalaxyGenerator.systems[last.GetLastQuestPath().solarName];
            last.GetLastQuestPath().targetName = system.stations[rnd.Next(0, system.stations.Count)].name;

            return start;
        }


        public List<string> BuildPathRecursive(SolarSystem target, List<string> path, int minJumpsCount, Random rnd)
        {
            if (target.sibligs.Count <= 1) return null;
            var shuffled = rnd.Shuffle(target.sibligs);

            SolarSystem solar = null;
            do
            {
                solar = GalaxyGenerator.systems[shuffled[rnd.Next(0, shuffled.Count)].solarName];
            } while (solar == null || path.Contains(solar.name));

            path.Add(solar.name);

            if (path.Count > minJumpsCount && solar.stations.Count != 0)
            {
                return path;
            }
            
            return BuildPathRecursive(solar, path, minJumpsCount, rnd);
        }


        public void RegeneratePathFromPlayer()
        {
            var worldHandler = PlayerDataManager.Instance.WorldHandler;
            if (currentPath.Contains(worldHandler.CurrentSolarSystem.name)) return;


            if (IsEmptyQuest()) return;

            isOnLoading = true;
            var path = new List<string>();

            new GalacticPathfinder(worldHandler.ShipPlayer, path, PlayerDataManager.Instance.WorldHandler.CurrentSolarSystem, targetSolar, delegate(List<string> list)
            {
                currentPath = list;
                isOnLoading = false;
                worldHandler.ShipPlayer.AppliedQuests.OnChangeQuests.Run();
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