using System;
using System.Collections.Generic;
using System.Text;
using Quests;
using UnityEngine;
using Random = System.Random;

namespace Quests
{
    [System.Serializable]
    public class QuestPath
    {
        public string solarName, targetName;
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
            Kill, Mine, Transfer
        }
        public Character quester;
        public QuestType questType;
        public int questID;
        public QuestPath pathToTarget = new QuestPath();
        public bool brokedQuest, isComplited, isRewarded;
        public Quest(System.Random rnd, Character character, string stationName)
        {
            Init(rnd, character, stationName);
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
        public void Init(System.Random rnd, Character character, string stationName)
        {
            quester = character;
            questType = (QuestType) rnd.Next(0, Enum.GetNames(typeof(QuestType)).Length);
            questID = rnd.Next(-9999999, 9999999);
            switch (questType)
            {
                case QuestType.Transfer:
                    InitTransfer(stationName);
                    break;
            }        
    
        }


        public void InitTransfer(string stationName)
        {
            System.Random rnd = new Random(questID);
            int pathLength = rnd.Next(0, 7);
            List<string> pathNames = new List<string>();
            QuestPath last = new QuestPath() {solarName = PlayerDataManager.CurrentSolarSystem.name};
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
                brokedQuest = true;
            }

            pathToTarget = first;
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

        public void Init(System.Random rnd)
        {
            characterID = rnd.Next(-9999999, 9999999);
            firstName = WorldOrbitalStation.ToUpperFist(WorldOrbitalStation.FirstNames[rnd.Next(0, WorldOrbitalStation.FirstNames.Length)]);
            lastName = WorldOrbitalStation.ToUpperFist(WorldOrbitalStation.LastNames[rnd.Next(0, WorldOrbitalStation.LastNames.Length)]);
            fraction = (Fraction)rnd.Next(0, Enum.GetNames(typeof(Fraction)).Length);
        }
    }
}



public class WorldOrbitalStation : MonoBehaviour
{
    public static WorldOrbitalStation Instance;
    public static string[] FirstNames, LastNames;
    [SerializeField] private int uniqSeed;
    public Transform spawnPoint;
    public List<Quest> quests;
    public List<Character> characters;
    public static event Action OnInit = delegate {  };
    public static void InitNames()
    {
        if (FirstNames == null)
        {
            FirstNames = LoadFromFile("names");
            LastNames = LoadFromFile("lastnames");
        }
    }

    public static void ClearEvent()
    {
        OnInit = delegate {  };
        Instance = null;
    }
    public void Init()
    {
        Instance = this;
        InitNames();
        CalcSeed();
        characters = InitCharacters(uniqSeed);
        quests = InitQuests(uniqSeed, characters);
        
        characters.AddRange(SpawnQuestCharacters());
        quests.AddRange(GetTargetStationQuests());
    }
    
    private static string[] LoadFromFile(string nm)
    {
        TextAsset mytxtData = (TextAsset) Resources.Load(nm);
        var wrds = mytxtData.text;
        return wrds.Split('/');
    }


    private void Start()
    {
        OnInit();
    }

    public void CalcSeed()
    {
        uniqSeed = 0;
        foreach (var ch in Encoding.ASCII.GetBytes(transform.name))
        {
            uniqSeed += ch;
        }

        uniqSeed *= uniqSeed;
    }

    public List<Character> InitCharacters(int seed)
    {
        var list = new List<Character>();
        Random rnd = new Random(seed);
        int _characters = rnd.Next(1, 5);
        for (int i = 0; i < _characters; i++)
        {
            var ch = new Character(rnd);
            list.Add(ch);
        }
        return list;
    }

    public List<Character> SpawnQuestCharacters()
    {
        var list = new List<Character>();
        List<Quest> questInStation = GetTargetStationQuests();
        
        for (int i = 0; i < questInStation.Count; i++)
        {
            questInStation[i].quester.firstName = questInStation[i].quester.firstName;
            list.Add(questInStation[i].quester);
        }

        return list;
    }

    public List<Quest> GetTargetStationQuests()
    {
        List<Quest> questInStation = new List<Quest>();
        
        foreach (var quest in AppliedQuests.Instance.quests)
        {
            if (quest.GetLastQuestPath().targetName == transform.name)
            {
                if (quest.GetLastQuestPath().solarName == PlayerDataManager.CurrentSolarSystem.name)
                {
                    if (questInStation.Find(x => x.quester.characterID == quest.quester.characterID) == null)
                    {
                        questInStation.Add(quest);
                    }
                }
            }
        }
        return questInStation;
    }

    public List<Quest> InitQuests(int seed, List<Character> _characters)
    {
        var list = new List<Quest>();
        Random rnd = new Random(seed);
        int quests = rnd.Next(6, 20);
        for (int i = 0; i < quests; i++)
        {
            var q = new Quest(rnd, _characters[rnd.Next(0, _characters.Count)], transform.name);
            if (!q.brokedQuest)
            {
                list.Add(q);
            }
        }

        return list;
    }

    public static string ToUpperFist(string str)
    {
        if (str.Length == 1)
            return char.ToUpper(str[0]).ToString();
        else
            return (char.ToUpper(str[0]) + str.Substring(1).ToLower()).ToString();
    }
}
