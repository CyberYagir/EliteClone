using System;
using System.Collections.Generic;
using Core.PlayerScripts;
using Core.Quests;
using Core.Systems;
using Core.Systems.InteractivePoints;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    public class WorldOrbitalStation : StartupObject
    {
        [Serializable]
        public class OrbitalStationMesh
        {
            [SerializeField] private GameObject worldObject;
            [SerializeField] private Transform spawnPoint;
            [SerializeField] private WorldOrbitalStationPoints points;

            public GameObject WorldObject => worldObject;
            public Transform SpawnPoint => spawnPoint;
            public WorldOrbitalStationPoints Points => points;
        }
        
        [SerializeField] private List<OrbitalStationMesh> meshList;
        [SerializeField] private int uniqSeed;
        [SerializeField] private StationRefiller refiller;
        [SerializeField] private QuestsMethodObject methods;

        public List<Quest> quests;
        public List<Character> characters;
        public List<int> additionalCharacters = new List<int>();

        private WorldOrbitalStationPoints points;
        private WorldDataHandler worldHandler;
        private SolarSystemShips ships;
        
        public WorldOrbitalStationPoints Points => points;
        

        public override void Init(PlayerDataManager playerDataManager)
        {
            base.Init(playerDataManager);
            
            worldHandler = playerDataManager.WorldHandler;

            OrbitalStationStaticBuilder.InitNames();
            uniqSeed = OrbitalStationStaticBuilder.CalcSeed(transform.name, LocationGenerator.CurrentSave.GetSystemCode());

            RandomVisuals();
            
            
            refiller.InitRefiller(uniqSeed);
            characters = InitCharacters(uniqSeed);
            quests = InitQuests(uniqSeed, characters);
        
            characters.AddRange(SpawnQuestCharacters());
            quests.AddRange(GetTargetStationQuests());

            GetComponent<ContactObject>().Init(false);
        
            RemoveCharactersWithoutQuests();
            
            
            points.Init(this, ships);
            
            Debug.LogError("InitStation");
        }

        public void SetShips(SolarSystemShips ships)
        {
            this.ships = ships;
        }

        private void RandomVisuals()
        {
            int meshType = new Random(uniqSeed).Next(0, meshList.Count);
            for (int i = 0; i < meshList.Count; i++)
            {
                if (i == meshType)
                {
                    meshList[i].WorldObject.SetActive(true);
                    GetComponent<WorldInteractivePoint>().spawnPoint = meshList[i].SpawnPoint;
                    points = meshList[i].Points;
                }
                else
                {
                    meshList[i].WorldObject.SetActive(false);
                }
            }
        }

        public int GetUniqSeed() => uniqSeed;

        

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
            
                if (quest.targetStructure == transform.name)
                {
                    if (quest.targetSolar == worldHandler.CurrentSolarSystem.name)
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
                var q = new Quest(questid, _characters[rnd.Next(0, _characters.Count)], transform.name, worldHandler.CurrentSolarSystem.name, methods);
                if (q.questState != Quest.QuestCompleted.BrokeQuest)
                {
                    list.Add(q);
                }
            }

            return list;
        }
    }
}