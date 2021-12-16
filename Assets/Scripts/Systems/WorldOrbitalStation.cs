using System;
using System.Collections.Generic;
using System.Text;
using Quests;
using UnityEngine;
using Random = System.Random;

namespace Quests
{
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

        public Quest(System.Random rnd, Character character)
        {
            Init(rnd, character);
        }

        public void Init(System.Random rnd, Character character)
        {
            quester = character;
            questType = (QuestType)rnd.Next(0, Enum.GetNames(typeof(QuestType)).Length);
            questID = rnd.Next(-9999999, 9999999);
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
        
        public Fraction fraction;

        public Character(System.Random rnd)
        {
            Init(rnd);
        }

        public void Init(System.Random rnd)
        {
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

    public List<Quest> InitQuests(int seed, List<Character> _characters)
    {
        var list = new List<Quest>();
        Random rnd = new Random(seed);
        int quests = rnd.Next(1, 20);
        for (int i = 0; i < quests; i++)
        {
            var q = new Quest(rnd, _characters[rnd.Next(0, _characters.Count)]);
            list.Add(q);
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
