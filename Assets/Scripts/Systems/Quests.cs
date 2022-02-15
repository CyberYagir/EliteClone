
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Quests
{
    [System.Serializable]
    public class QuestPath
    {
        public string solarName = "", targetName = "";
        public QuestPath prevPath, nextPath;
        public QuestPath()
        {
            
        }
        
        public bool isFirst
        {
            get
            {
                return prevPath == null;
            }
        }

        public bool isLast
        {
            get
            {
                return nextPath == null;
            }
        }
    }

    [System.Serializable]
    public class Quest
    {
        public enum QuestType
        {
            Kill,
            Mine,
            Transfer
        }
        public enum QuestComplited
        {
            None, BrokeQuest, Complited, Rewarded
        }
        
        public Character quester;
        public QuestType questType;
        public int questID;
        public QuestPath pathToTarget = new QuestPath();
        public QuestComplited questState;

        public Reward reward = new Reward();
        public string appliedStation, appliedSolar;
        public List<Item> toTransfer = new List<Item>();

        public Quest(int questSeed , Character character, string stationName, string appliedSolar)
        {
            appliedStation = stationName;
            this.appliedSolar = appliedSolar;
            
            Init(questSeed, character, stationName, appliedSolar);
        }

        public Quest()
        {

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

        public void Init(int questSeed, Character character, string stationName, string solarName)
        {
            var rnd = new Random(questSeed);
            quester = character;
            questType = (QuestType) rnd.Next(0, Enum.GetNames(typeof(QuestType)).Length);
            questID = questSeed; 
            switch (questType)
            {
                case QuestType.Transfer:
                    InitTransfer(stationName, solarName);
                    reward.Init(questID);
                    break;
                case QuestType.Mine:
                    InitMine(stationName, solarName);
                    var cost = 0;
                    for (int i = 0; i < toTransfer.Count; i++)
                    {
                        cost += (int)toTransfer[i].amount.value * rnd.Next(15, 25);
                    }
                    reward.Init(questID, cost);
                    break;
            }

        }

        
        
        public void CheckIsQuestCompleted()
        {
            if (questType == QuestType.Transfer || questType == QuestType.Mine)
            {
                bool allItemInInventory = IsHaveAllItems();
                var last = GetLastQuestPath();
                if (allItemInInventory)
                {
                    if (last.solarName == PlayerDataManager.CurrentSolarSystem.name)
                    {
                        if (WorldOrbitalStation.Instance.transform.name == last.targetName)
                        {
                            questState = QuestComplited.Complited;
                        }
                    }
                }
            }
        }

        public bool IsHaveAllItems()
        {
            bool allItemInInventory = true;
            List<Item> chekedItems = new List<Item>();
            for (int i = 0; i < toTransfer.Count; i++)
            {
                if (!Player.inst.cargo.ContainItem(toTransfer[i].id.idname))
                {
                    allItemInInventory = false;
                }
                else
                {
                    var findItem = Player.inst.cargo.FindItem(toTransfer[i].id.idname);
                    if (!chekedItems.Contains(findItem))
                    {
                        if (findItem.amount.value >= toTransfer[i].amount.value)
                        {
                            chekedItems.Add(findItem);
                        }
                        else
                        {
                            allItemInInventory = false;
                        }
                    }
                    else
                    {
                        allItemInInventory = false;
                    }
                }
            }
            
            return allItemInInventory;
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

        public void InitTransfer(string stationName, string solarName)
        {
            System.Random rnd = new Random(questID);


            int transfersCount = rnd.Next(1, 3);
            for (int i = 0; i < transfersCount; i++)
            {
                toTransfer.Add(ItemsManager.GetTransferedItem(rnd));
            }

            pathToTarget = GetPath(rnd, stationName, solarName);
        }
        public void InitMine(string stationName, string solarName)
        {
            System.Random rnd = new Random(questID);
            int transfersCount = rnd.Next(1, 3);
            for (int i = 0; i < transfersCount; i++)
            {
                var mineral = ItemsManager.GetMineralItem(rnd);
                mineral.amount.SetValue(mineral.amount.Max);
                toTransfer.Add(mineral);
            }

            pathToTarget = new QuestPath() { solarName = solarName, targetName = stationName};
        }
    }

    public class Reward
    {
        public enum RewardType
        {
            Money, Item, Items
        }

        public RewardType type;
        public List<Item> rewardItems = new List<Item>();

        public void Init(int questID, float settedMoney = -1)
        {
            var rnd = new Random(questID);
            type = (RewardType) rnd.Next(0, Enum.GetNames(typeof(RewardType)).Length);
            
            if (type == RewardType.Money)
            {
                var money = ItemsManager.GetCredits().Clone();
                money.amount.SetValue(rnd.Next(5000, 20000));
                rewardItems.Add(money);
            }
            else
            {
                int count = 1;
                if (type == RewardType.Items)
                {
                    count += rnd.Next(0, 2);
                }
                for (int i = 0; i < count; i++)
                {
                    rewardItems.Add(ItemsManager.GetRewardItem(rnd));
                }
                var money = ItemsManager.GetCredits().Clone();
                if (settedMoney == -1)
                {
                    money.amount.SetValue(rnd.Next(1000, 3000));
                }
                else
                {
                    money.amount.SetValue(settedMoney);
                }

                rewardItems.Add(money);
            }
        }
    }

    public enum Fraction
    {
        Pirates,
        Libertarians,
        Communists,
        Anarchists,
        OCG
    }
    [System.Serializable]
    public class Character
    {
        public string firstName;
        public string lastName;
        public int characterID;
        
        
        public Fraction fraction;

        public Character(System.Random rnd)
        {
            Init(rnd);
        }

        public Character()
        {
            
        }

        public void Reset()
        {
            firstName = null;
            lastName = null;
        }
        public void Init(System.Random rnd)
        {
            characterID = rnd.Next(-9999999, 9999999);
            firstName = WorldOrbitalStation.ToUpperFist(WorldOrbitalStation.FirstNames[rnd.Next(0, WorldOrbitalStation.FirstNames.Length)]);
            lastName = WorldOrbitalStation.ToUpperFist(WorldOrbitalStation.LastNames[rnd.Next(0, WorldOrbitalStation.LastNames.Length)]);
            fraction = (Fraction)rnd.Next(0, Enum.GetNames(typeof(Fraction)).Length);
        }
    }
}


