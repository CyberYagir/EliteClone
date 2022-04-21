using System.Collections.Generic;
using System.Text;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    public class NamesHolder
    {
        public static NamesHolder Instance;
        public string[] FirstNames, LastNames;


        public static void Init()
        {
            if (Instance == null)
            {
                Instance = new NamesHolder();
                if (Instance.FirstNames == null)
                {
                    Instance.FirstNames = LoadFromFile("names");
                    Instance.LastNames = LoadFromFile("lastnames");
                }
            }
        }

        public static string GetFirstName(Random rnd)
        {
            Init();
            return Instance.FirstNames[rnd.Next(0, Instance.FirstNames.Length)];
        }
        public static string GetLastName(Random rnd)
        {
            Init();
            return Instance.LastNames[rnd.Next(0, Instance.LastNames.Length)];
        }
        private static string[] LoadFromFile(string nm)
        {
            TextAsset mytxtData = (TextAsset) Resources.Load(nm);
            var wrds = mytxtData.text;
            return wrds.Split('/');
        }
    
        public static string ToUpperFist(string str)
        {
            if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            return (char.ToUpper(str[0]) + str.Substring(1).ToLower());
        }

        public static int StringToSeed(string name)
        {
            int seed = 0;
            foreach (var ch in Encoding.ASCII.GetBytes(name))
            {
                seed += ch;
            }

            return seed;
        }
    }

    public class WorldOrbitalStation : MonoBehaviour
    {
        public static WorldOrbitalStation Instance;
        [SerializeField] private int uniqSeed;
        [SerializeField] private StationRefiller refiller;
        public List<Quest> quests;
        public List<Character> characters;
        public List<int> additionalCharacters = new List<int>();

        public static Event OnInit = new Event();

        public static void InitNames()
        {
            NamesHolder.Init();
        }

        public static void ClearEvent()
        {
            OnInit = new Event();
            Instance = null;
        }
        public void Init()
        {
            Instance = this;
            InitNames();
            uniqSeed = CalcSeed(transform.name, LocationGenerator.CurrentSave.GetSystemCode());
            refiller.InitRefiller(uniqSeed);
            characters = InitCharacters(uniqSeed);
            quests = InitQuests(uniqSeed, characters);
        
            characters.AddRange(SpawnQuestCharacters());
            quests.AddRange(GetTargetStationQuests());

            GetComponent<ContactObject>().Init(false);
        
            RemoveCharactersWithoutQuests();
        }

        public int GetUniqSeed()
        {
            return uniqSeed;
        }



        private void Start()
        {
            OnInit.Run();
        }

        public static int CalcSeed(string stationName, string systemName)
        {
            int uniqSeed = NamesHolder.StringToSeed(stationName + systemName);
            uniqSeed *= uniqSeed;
            return uniqSeed;
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
                if (characters.Find(x => x.characterID == questInStation[i].quester.characterID) == null)
                {
                    list.Add(questInStation[i].quester);
                    additionalCharacters.Add(questInStation[i].quester.characterID);
                }
            }

            return list;
        }

        public void RemoveCharactersWithoutQuests()
        {
            foreach (var chr in characters)
            {
                if (quests.FindAll(x => x.quester.characterID == chr.characterID && x.questState != Quest.QuestCompleted.Rewarded).Count == 0) 
                {
                    characters.Remove(chr);
                    RemoveCharactersWithoutQuests();
                    break;
                }
            }
        }
    
        public List<Quest> GetTargetStationQuests()
        {
            List<Quest> questInStation = new List<Quest>();
        
            foreach (var quest in AppliedQuests.Instance.quests)
            {
                if (quest.questState == Quest.QuestCompleted.Rewarded) continue;
            
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
                var questid = rnd.Next(-9999999, 9999999);
                var q = new Quest(questid, _characters[rnd.Next(0, _characters.Count)], transform.name, PlayerDataManager.CurrentSolarSystem.name);
                if (q.questState != Quest.QuestCompleted.BrokeQuest)
                {
                    list.Add(q);
                }
            }

            return list;
        }
    }
}